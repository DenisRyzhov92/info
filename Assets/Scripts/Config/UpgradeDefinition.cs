using UnityEngine;

/// <summary>
/// Определение апгрейда для Space Farm Idle Clicker.
/// Используется в ScriptableObject конфиге для настройки всех апгрейдов игры.
/// </summary>
[System.Serializable]
public class UpgradeDefinition
{
    [Header("Основная информация")]
    [Tooltip("Уникальный ID апгрейда (например, 'manual_harvest', 'drone_swarm')")]
    public string id;

    [Tooltip("Название апгрейда для отображения в UI")]
    public string displayName;

    [Tooltip("Описание апгрейда для UI")]
    [TextArea(2, 4)]
    public string description;

    [Header("Экономика")]
    [Tooltip("Базовая стоимость первого уровня")]
    public long baseCost = 10;

    [Tooltip("Множитель стоимости за каждый уровень (1.5 = цена растёт в 1.5 раза)")]
    [Range(1.1f, 3f)]
    public float costMultiplier = 1.7f;

    [Header("Бонусы")]
    [Tooltip("Тип бонуса: TapBoost, IdleIncome, IncomeMultiplier")]
    public UpgradeBonusType bonusType;

    [Tooltip("Значение бонуса за уровень (для TapBoost и IdleIncome - абсолютное значение, для Multiplier - множитель)")]
    public float bonusValue = 1f;

    [Header("Unlock")]
    [Tooltip("Минимальный lifetime BioGel для разблокировки этого апгрейда")]
    public long unlockLifetimeBioGel = 0;

    /// <summary>
    /// Рассчитывает стоимость апгрейда для указанного уровня.
    /// </summary>
    public long GetCostForLevel(int level)
    {
        float cost = baseCost * Mathf.Pow(costMultiplier, level);
        return Mathf.CeilToInt(cost);
    }

    /// <summary>
    /// Проверяет, разблокирован ли апгрейд для указанного lifetime BioGel.
    /// </summary>
    public bool IsUnlocked(long lifetimeBioGel)
    {
        return lifetimeBioGel >= unlockLifetimeBioGel;
    }
}

/// <summary>
/// Тип бонуса от апгрейда.
/// </summary>
public enum UpgradeBonusType
{
    TapBoost,           // Увеличивает bioGelPerTap
    IdleIncome,         // Увеличивает bioGelPerSecond
    IncomeMultiplier    // Множитель ко всему доходу (tap + idle)
}
