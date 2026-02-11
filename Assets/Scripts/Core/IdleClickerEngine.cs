using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

/// <summary>
/// Ядро экономики Space Farm Idle Clicker.
/// Управляет ресурсами, апгрейдами, бустами и расчётами дохода.
/// Не зависит от Unity MonoBehaviour - чистая логика.
/// </summary>
public class IdleClickerEngine
{
    // Ресурсы
    public long BioGel { get; private set; }
    public long LifetimeBioGel { get; private set; }
    public long BioGelPerTap { get; private set; }
    public float BioGelPerSecond { get; private set; }

    // Конфиг
    private IdleClickerConfig _config;

    // Уровни апгрейдов (upgradeId -> level)
    private Dictionary<string, int> _upgradeLevels = new Dictionary<string, int>();

    // Активные бусты (boostId -> endTimestamp)
    private Dictionary<string, long> _activeBoosts = new Dictionary<string, long>();

    // События для UI
    public event Action OnBioGelChanged;
    public event Action OnUpgradePurchased;
    public event Action OnBoostActivated;

    public IdleClickerEngine(IdleClickerConfig config)
    {
        _config = config;
        BioGel = config.startingBioGel;
        BioGelPerTap = config.startingBioGelPerTap;
        BioGelPerSecond = config.startingBioGelPerSecond;
    }

    /// <summary>
    /// Загружает данные из сохранения.
    /// </summary>
    public void LoadFromSave(IdleClickerSaveData saveData)
    {
        BioGel = saveData.bioGel;
        LifetimeBioGel = saveData.lifetimeBioGel;
        BioGelPerTap = saveData.bioGelPerTap;
        BioGelPerSecond = saveData.bioGelPerSecond;
        _upgradeLevels = saveData.upgradeLevels ?? new Dictionary<string, int>();

        // Восстанавливаем активные бусты
        _activeBoosts.Clear();
        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        foreach (var boostData in saveData.activeBoosts ?? new List<ActiveBoostData>())
        {
            if (boostData.endTimestamp > currentTimestamp)
            {
                _activeBoosts[boostData.boostId] = boostData.endTimestamp;
            }
        }

        RecalculateStats();
    }

    /// <summary>
    /// Сохраняет данные в формат сохранения.
    /// </summary>
    public IdleClickerSaveData CreateSaveData()
    {
        var saveData = new IdleClickerSaveData
        {
            bioGel = BioGel,
            lifetimeBioGel = LifetimeBioGel,
            bioGelPerTap = BioGelPerTap,
            bioGelPerSecond = BioGelPerSecond,
            upgradeLevels = new Dictionary<string, int>(_upgradeLevels),
            activeBoosts = new List<ActiveBoostData>()
        };

        // Сохраняем активные бусты
        foreach (var kvp in _activeBoosts)
        {
            saveData.activeBoosts.Add(new ActiveBoostData
            {
                boostId = kvp.Key,
                endTimestamp = kvp.Value
            });
        }

        return saveData;
    }

    /// <summary>
    /// Тап по полю (ручной сбор BioGel).
    /// </summary>
    public void Tap()
    {
        long tapIncome = GetTapIncome();
        AddBioGel(tapIncome);
    }

    /// <summary>
    /// Добавляет BioGel (с учётом lifetime).
    /// </summary>
    public void AddBioGel(long amount)
    {
        if (amount <= 0) return;
        BioGel += amount;
        LifetimeBioGel += amount;
        OnBioGelChanged?.Invoke();
    }

    /// <summary>
    /// Покупает апгрейд по ID.
    /// </summary>
    public bool BuyUpgrade(string upgradeId)
    {
        var upgrade = _config.GetUpgradeById(upgradeId);
        if (upgrade == null)
        {
            Debug.LogError($"[IdleClickerEngine] Апгрейд '{upgradeId}' не найден в конфиге");
            return false;
        }

        // Проверка unlock
        if (!upgrade.IsUnlocked(LifetimeBioGel))
        {
            Debug.LogWarning($"[IdleClickerEngine] Апгрейд '{upgradeId}' ещё не разблокирован");
            return false;
        }

        int currentLevel = GetUpgradeLevel(upgradeId);
        long cost = upgrade.GetCostForLevel(currentLevel);

        if (BioGel < cost)
        {
            return false; // Не хватает BioGel
        }

        BioGel -= cost;
        _upgradeLevels[upgradeId] = currentLevel + 1;
        RecalculateStats();
        OnUpgradePurchased?.Invoke();
        OnBioGelChanged?.Invoke();
        return true;
    }

    /// <summary>
    /// Покупает временный буст за BioGel.
    /// </summary>
    public bool BuyBoost(string boostId)
    {
        var boost = _config.GetBoostById(boostId);
        if (boost == null)
        {
            Debug.LogError($"[IdleClickerEngine] Буст '{boostId}' не найден в конфиге");
            return false;
        }

        // Проверка unlock
        if (!boost.unlockLifetimeBioGel.Equals(0) && LifetimeBioGel < boost.unlockLifetimeBioGel)
        {
            Debug.LogWarning($"[IdleClickerEngine] Буст '{boostId}' ещё не разблокирован");
            return false;
        }

        if (BioGel < boost.cost)
        {
            return false; // Не хватает BioGel
        }

        BioGel -= boost.cost;
        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long endTimestamp = currentTimestamp + boost.durationSeconds;
        _activeBoosts[boostId] = endTimestamp;
        OnBoostActivated?.Invoke();
        OnBioGelChanged?.Invoke();
        return true;
    }

    /// <summary>
    /// Обновляет активные бусты (удаляет истёкшие).
    /// </summary>
    public void UpdateActiveBoosts()
    {
        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var expiredBoosts = _activeBoosts.Where(kvp => kvp.Value <= currentTimestamp).ToList();
        foreach (var kvp in expiredBoosts)
        {
            _activeBoosts.Remove(kvp.Key);
        }
    }

    /// <summary>
    /// Получает текущий доход за тап (с учётом бустов).
    /// </summary>
    public long GetTapIncome()
    {
        float multiplier = GetTapMultiplier();
        return Mathf.CeilToInt(BioGelPerTap * multiplier);
    }

    /// <summary>
    /// Получает текущий пассивный доход в секунду (с учётом бустов).
    /// </summary>
    public float GetIdleIncome()
    {
        float multiplier = GetIdleMultiplier();
        return BioGelPerSecond * multiplier;
    }

    /// <summary>
    /// Получает уровень апгрейда.
    /// </summary>
    public int GetUpgradeLevel(string upgradeId)
    {
        return _upgradeLevels.ContainsKey(upgradeId) ? _upgradeLevels[upgradeId] : 0;
    }

    /// <summary>
    /// Проверяет, разблокирован ли апгрейд.
    /// </summary>
    public bool IsUpgradeUnlocked(string upgradeId)
    {
        var upgrade = _config.GetUpgradeById(upgradeId);
        return upgrade != null && upgrade.IsUnlocked(LifetimeBioGel);
    }

    /// <summary>
    /// Проверяет, разблокирован ли буст.
    /// </summary>
    public bool IsBoostUnlocked(string boostId)
    {
        var boost = _config.GetBoostById(boostId);
        return boost != null && (boost.unlockLifetimeBioGel == 0 || LifetimeBioGel >= boost.unlockLifetimeBioGel);
    }

    /// <summary>
    /// Проверяет, активен ли буст.
    /// </summary>
    public bool IsBoostActive(string boostId)
    {
        if (!_activeBoosts.ContainsKey(boostId))
            return false;

        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return _activeBoosts[boostId] > currentTimestamp;
    }

    /// <summary>
    /// Пересчитывает статистику на основе апгрейдов.
    /// </summary>
    private void RecalculateStats()
    {
        long baseTap = _config.startingBioGelPerTap;
        float baseIdle = _config.startingBioGelPerSecond;
        float incomeMultiplier = 1f;

        foreach (var upgrade in _config.upgrades)
        {
            int level = GetUpgradeLevel(upgrade.id);
            if (level <= 0) continue;

            switch (upgrade.bonusType)
            {
                case UpgradeBonusType.TapBoost:
                    baseTap += (long)(upgrade.bonusValue * level);
                    break;
                case UpgradeBonusType.IdleIncome:
                    baseIdle += upgrade.bonusValue * level;
                    break;
                case UpgradeBonusType.IncomeMultiplier:
                    incomeMultiplier *= Mathf.Pow(upgrade.bonusValue, level);
                    break;
            }
        }

        BioGelPerTap = baseTap;
        BioGelPerSecond = baseIdle * incomeMultiplier;
    }

    /// <summary>
    /// Получает множитель тапа из активных бустов.
    /// </summary>
    private float GetTapMultiplier()
    {
        float multiplier = 1f;
        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        foreach (var kvp in _activeBoosts)
        {
            if (kvp.Value <= currentTimestamp) continue; // Буст истёк

            var boost = _config.GetBoostById(kvp.Key);
            if (boost == null) continue;

            if (boost.effectType == BoostEffectType.TapMultiplier || 
                boost.effectType == BoostEffectType.AllIncomeMultiplier)
            {
                multiplier *= boost.multiplier;
            }
        }

        return multiplier;
    }

    /// <summary>
    /// Получает множитель пассивного дохода из активных бустов.
    /// </summary>
    private float GetIdleMultiplier()
    {
        float multiplier = 1f;
        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        foreach (var kvp in _activeBoosts)
        {
            if (kvp.Value <= currentTimestamp) continue; // Буст истёк

            var boost = _config.GetBoostById(kvp.Key);
            if (boost == null) continue;

            if (boost.effectType == BoostEffectType.IdleMultiplier || 
                boost.effectType == BoostEffectType.AllIncomeMultiplier)
            {
                multiplier *= boost.multiplier;
            }
        }

        return multiplier;
    }
}
