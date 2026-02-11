using UnityEngine;

/// <summary>
/// Определение IAP продукта за реальные деньги.
/// Используется в ScriptableObject конфиге для настройки магазина реальных покупок.
/// </summary>
[System.Serializable]
public class RealMoneyProductDefinition
{
    [Header("Основная информация")]
    [Tooltip("Product ID для магазина (Google Play / App Store)")]
    public string productId;

    [Tooltip("Название продукта для отображения в UI")]
    public string displayName;

    [Tooltip("Описание продукта для UI")]
    [TextArea(2, 4)]
    public string description;

    [Header("Награды")]
    [Tooltip("Количество BioGel, которое получает игрок")]
    public long bioGelReward = 0;

    [Tooltip("Список ID бустов, которые получает игрок (например, 'ion_pulse', 'solar_focus')")]
    public string[] boostRewards = new string[0];

    [Tooltip("Количество каждого буста")]
    public int[] boostRewardCounts = new int[0];
}
