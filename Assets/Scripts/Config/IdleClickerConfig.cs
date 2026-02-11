using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Главный конфиг Space Farm Idle Clicker.
/// ScriptableObject со всеми настройками апгрейдов, бустов и IAP продуктов.
/// </summary>
[CreateAssetMenu(fileName = "IdleClickerConfig", menuName = "Space Farm/Idle Clicker Config", order = 1)]
public class IdleClickerConfig : ScriptableObject
{
    [Header("Стартовые параметры")]
    [Tooltip("Начальное количество BioGel")]
    public long startingBioGel = 0;

    [Tooltip("Начальный доход за тап")]
    public long startingBioGelPerTap = 1;

    [Tooltip("Начальный пассивный доход в секунду")]
    public float startingBioGelPerSecond = 0f;

    [Header("Апгрейды")]
    [Tooltip("Список всех апгрейдов игры")]
    public List<UpgradeDefinition> upgrades = new List<UpgradeDefinition>();

    [Header("Магазин бустов за BioGel")]
    [Tooltip("Список всех временных бустов")]
    public List<ProgressBoostOfferDefinition> boostOffers = new List<ProgressBoostOfferDefinition>();

    [Header("IAP продукты")]
    [Tooltip("Список всех продуктов за реальные деньги")]
    public List<RealMoneyProductDefinition> realMoneyProducts = new List<RealMoneyProductDefinition>();

    /// <summary>
    /// Находит апгрейд по ID.
    /// </summary>
    public UpgradeDefinition GetUpgradeById(string id)
    {
        return upgrades.Find(u => u.id == id);
    }

    /// <summary>
    /// Находит буст по ID.
    /// </summary>
    public ProgressBoostOfferDefinition GetBoostById(string id)
    {
        return boostOffers.Find(b => b.id == id);
    }

    /// <summary>
    /// Находит IAP продукт по Product ID.
    /// </summary>
    public RealMoneyProductDefinition GetProductById(string productId)
    {
        return realMoneyProducts.Find(p => p.productId == productId);
    }
}
