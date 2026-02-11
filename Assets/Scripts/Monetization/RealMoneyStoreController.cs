using UnityEngine;
using System;

/// <summary>
/// Контроллер магазина реальных покупок (IAP).
/// Управляет покупкой продуктов и выдачей наград игроку.
/// </summary>
public class RealMoneyStoreController : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("IdleClickerManager (если не задан, будет найден автоматически)")]
    public IdleClickerManager manager;

    [Tooltip("Провайдер IAP (MockIapProvider для тестирования, UnityIapProvider для реальных покупок)")]
    public IapProviderBase iapProvider;

    private void Start()
    {
        if (manager == null)
            manager = IdleClickerManager.Instance;

        if (iapProvider == null)
        {
            Debug.LogWarning("[RealMoneyStoreController] IAP провайдер не назначен! Используется Mock провайдер.");
            iapProvider = gameObject.AddComponent<MockIapProvider>();
        }

        // Инициализируем провайдер
        if (iapProvider is IIapProvider provider)
        {
            provider.Initialize(OnIapInitialized);
        }
    }

    /// <summary>
    /// Покупает IAP продукт по Product ID.
    /// </summary>
    public void PurchaseProduct(string productId)
    {
        if (manager == null || manager.config == null)
        {
            Debug.LogError("[RealMoneyStoreController] IdleClickerManager или конфиг не найден!");
            return;
        }

        var product = manager.config.GetProductById(productId);
        if (product == null)
        {
            Debug.LogError($"[RealMoneyStoreController] Продукт '{productId}' не найден в конфиге!");
            return;
        }

        if (iapProvider == null || !(iapProvider is IIapProvider provider))
        {
            Debug.LogError("[RealMoneyStoreController] IAP провайдер не инициализирован!");
            return;
        }

        provider.PurchaseProduct(productId, (success, purchasedProductId) =>
        {
            if (success)
            {
                GiveProductRewards(product);
                Debug.Log($"[RealMoneyStoreController] Продукт '{productId}' успешно куплен!");
            }
            else
            {
                Debug.LogWarning($"[RealMoneyStoreController] Покупка продукта '{productId}' не удалась");
            }
        });
    }

    /// <summary>
    /// Выдаёт награды за купленный продукт.
    /// </summary>
    private void GiveProductRewards(RealMoneyProductDefinition product)
    {
        if (manager == null || manager.GetEngine() == null) return;

        var engine = manager.GetEngine();

        // Выдаём BioGel
        if (product.bioGelReward > 0)
        {
            engine.AddBioGel(product.bioGelReward);
        }

        // Выдаём бусты
        if (product.boostRewards != null && product.boostRewardCounts != null)
        {
            for (int i = 0; i < product.boostRewards.Length && i < product.boostRewardCounts.Length; i++)
            {
                string boostId = product.boostRewards[i];
                int count = product.boostRewardCounts[i];

                for (int j = 0; j < count; j++)
                {
                    // Активируем буст (в реальной игре бусты должны добавляться в инвентарь,
                    // но для простоты сразу активируем)
                    engine.BuyBoost(boostId);
                }
            }
        }

        manager.SaveProgress();
    }

    /// <summary>
    /// Восстанавливает покупки (для iOS).
    /// </summary>
    public void RestorePurchases()
    {
        if (iapProvider == null || !(iapProvider is IIapProvider provider))
        {
            Debug.LogError("[RealMoneyStoreController] IAP провайдер не инициализирован!");
            return;
        }

        provider.RestorePurchases((success) =>
        {
            if (success)
            {
                Debug.Log("[RealMoneyStoreController] Покупки успешно восстановлены!");
            }
            else
            {
                Debug.LogWarning("[RealMoneyStoreController] Восстановление покупок не удалось");
            }
        });
    }

    private void OnIapInitialized(bool success)
    {
        if (success)
        {
            Debug.Log("[RealMoneyStoreController] IAP провайдер успешно инициализирован");
        }
        else
        {
            Debug.LogError("[RealMoneyStoreController] Ошибка инициализации IAP провайдера");
        }
    }
}
