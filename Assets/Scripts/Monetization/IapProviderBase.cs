using UnityEngine;
using System;

/// <summary>
/// Базовый интерфейс для провайдера IAP (In-App Purchases).
/// Реализации: MockIapProvider (для тестирования), UnityIapProvider (для реальных покупок).
/// </summary>
public interface IIapProvider
{
    /// <summary>
    /// Инициализирует провайдер IAP.
    /// </summary>
    void Initialize(Action<bool> onInitialized);

    /// <summary>
    /// Покупает продукт по Product ID.
    /// </summary>
    void PurchaseProduct(string productId, Action<bool, string> onPurchaseComplete);

    /// <summary>
    /// Восстанавливает покупки (для iOS).
    /// </summary>
    void RestorePurchases(Action<bool> onRestoreComplete);

    /// <summary>
    /// Проверяет, инициализирован ли провайдер.
    /// </summary>
    bool IsInitialized { get; }
}

/// <summary>
/// Базовый класс для провайдера IAP.
/// </summary>
public abstract class IapProviderBase : MonoBehaviour, IIapProvider
{
    public abstract bool IsInitialized { get; }

    public abstract void Initialize(Action<bool> onInitialized);
    public abstract void PurchaseProduct(string productId, Action<bool, string> onPurchaseComplete);
    public abstract void RestorePurchases(Action<bool> onRestoreComplete);
}
