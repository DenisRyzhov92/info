using UnityEngine;
using System;

/// <summary>
/// Mock провайдер IAP для локального тестирования без реальных покупок.
/// Всегда успешно выполняет покупки для тестирования логики игры.
/// </summary>
public class MockIapProvider : IapProviderBase
{
    private bool _isInitialized = false;

    public override bool IsInitialized => _isInitialized;

    public override void Initialize(Action<bool> onInitialized)
    {
        _isInitialized = true;
        Debug.Log("[MockIapProvider] Инициализирован (Mock режим)");
        onInitialized?.Invoke(true);
    }

    public override void PurchaseProduct(string productId, Action<bool, string> onPurchaseComplete)
    {
        if (!_isInitialized)
        {
            Debug.LogWarning("[MockIapProvider] Провайдер не инициализирован!");
            onPurchaseComplete?.Invoke(false, "Not initialized");
            return;
        }

        // В Mock режиме всегда успешно
        Debug.Log($"[MockIapProvider] Mock покупка продукта: {productId}");
        onPurchaseComplete?.Invoke(true, productId);
    }

    public override void RestorePurchases(Action<bool> onRestoreComplete)
    {
        Debug.Log("[MockIapProvider] Mock восстановление покупок (всегда успешно)");
        onRestoreComplete?.Invoke(true);
    }
}
