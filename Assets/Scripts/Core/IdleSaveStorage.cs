using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

/// <summary>
/// Система сохранения/загрузки прогресса Space Farm Idle Clicker в JSON формате.
/// Сохраняет данные в Application.persistentDataPath/idle_clicker_save.json
/// </summary>
[System.Serializable]
public class IdleClickerSaveData
{
    public long bioGel = 0;
    public long lifetimeBioGel = 0;
    public long bioGelPerTap = 1;
    public float bioGelPerSecond = 0f;
    public System.Collections.Generic.Dictionary<string, int> upgradeLevels = new System.Collections.Generic.Dictionary<string, int>();
    public System.Collections.Generic.List<ActiveBoostData> activeBoosts = new System.Collections.Generic.List<ActiveBoostData>();
    public long lastSaveTimestamp = 0;
}

[System.Serializable]
public class ActiveBoostData
{
    public string boostId;
    public long endTimestamp; // Unix timestamp когда буст закончится
}

public static class IdleSaveStorage
{
    private const string SAVE_FILE_NAME = "idle_clicker_save.json";

    /// <summary>
    /// Сохраняет данные игры в JSON файл.
    /// </summary>
    public static void Save(IdleClickerSaveData data)
    {
        try
        {
            data.lastSaveTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string json = JsonUtility.ToJson(data, true);
            string filePath = GetSaveFilePath();
            File.WriteAllText(filePath, json);
            Debug.Log($"[IdleSaveStorage] Сохранено в {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[IdleSaveStorage] Ошибка сохранения: {e.Message}");
        }
    }

    /// <summary>
    /// Загружает данные игры из JSON файла.
    /// </summary>
    public static IdleClickerSaveData Load()
    {
        try
        {
            string filePath = GetSaveFilePath();
            if (!File.Exists(filePath))
            {
                Debug.Log("[IdleSaveStorage] Файл сохранения не найден, создан новый");
                return new IdleClickerSaveData();
            }

            string json = File.ReadAllText(filePath);
            IdleClickerSaveData data = JsonUtility.FromJson<IdleClickerSaveData>(json);
            
            // Инициализируем словари если они null (для совместимости со старыми сохранениями)
            if (data.upgradeLevels == null)
                data.upgradeLevels = new System.Collections.Generic.Dictionary<string, int>();
            if (data.activeBoosts == null)
                data.activeBoosts = new System.Collections.Generic.List<ActiveBoostData>();

            Debug.Log($"[IdleSaveStorage] Загружено из {filePath}");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"[IdleSaveStorage] Ошибка загрузки: {e.Message}");
            return new IdleClickerSaveData();
        }
    }

    /// <summary>
    /// Удаляет файл сохранения (для тестирования).
    /// </summary>
    public static void DeleteSave()
    {
        try
        {
            string filePath = GetSaveFilePath();
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("[IdleSaveStorage] Файл сохранения удалён");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[IdleSaveStorage] Ошибка удаления: {e.Message}");
        }
    }

    private static string GetSaveFilePath()
    {
        return Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
    }

    /// <summary>
    /// Рассчитывает оффлайн-доход на основе времени отсутствия.
    /// </summary>
    /// <param name="lastSaveTimestamp">Unix timestamp последнего сохранения</param>
    /// <param name="bioGelPerSecond">Текущий пассивный доход в секунду</param>
    /// <param name="maxOfflineHours">Максимальное количество часов оффлайн-дохода (по умолчанию 24)</param>
    /// <returns>Количество BioGel за оффлайн время</returns>
    public static long CalculateOfflineIncome(long lastSaveTimestamp, float bioGelPerSecond, int maxOfflineHours = 24)
    {
        if (bioGelPerSecond <= 0f || lastSaveTimestamp <= 0)
            return 0;

        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long offlineSeconds = currentTimestamp - lastSaveTimestamp;

        // Максимум 24 часа оффлайн-дохода
        long maxOfflineSeconds = maxOfflineHours * 3600;
        if (offlineSeconds > maxOfflineSeconds)
            offlineSeconds = maxOfflineSeconds;

        // 50% от онлайн-дохода
        float offlineIncome = offlineSeconds * bioGelPerSecond * 0.5f;
        return Mathf.CeilToInt(offlineIncome);
    }
}
