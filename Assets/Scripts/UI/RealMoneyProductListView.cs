using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI компонент для отображения списка всех IAP продуктов.
/// </summary>
public class RealMoneyProductListView : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("RealMoneyStoreController (если не задан, будет найден автоматически)")]
    public RealMoneyStoreController storeController;

    [Tooltip("Prefab кнопки продукта")]
    public GameObject productButtonPrefab;

    [Tooltip("Родительский объект для размещения кнопок (обычно Content ScrollView)")]
    public Transform contentRoot;

    private List<RealMoneyProductButtonView> _productButtons = new List<RealMoneyProductButtonView>();

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Инициализирует список продуктов.
    /// </summary>
    public void Initialize()
    {
        if (storeController == null)
            storeController = FindObjectOfType<RealMoneyStoreController>();

        if (storeController == null || storeController.manager == null || storeController.manager.config == null)
        {
            Debug.LogError("[RealMoneyProductListView] RealMoneyStoreController или конфиг не найден!");
            return;
        }

        if (productButtonPrefab == null)
        {
            Debug.LogError("[RealMoneyProductListView] Prefab кнопки продукта не назначен!");
            return;
        }

        if (contentRoot == null)
        {
            Debug.LogError("[RealMoneyProductListView] Content Root не назначен!");
            return;
        }

        // Очищаем старые кнопки
        ClearProducts();

        // Создаём кнопки для каждого продукта
        foreach (var product in storeController.manager.config.realMoneyProducts)
        {
            GameObject buttonObj = Instantiate(productButtonPrefab, contentRoot);
            RealMoneyProductButtonView buttonView = buttonObj.GetComponent<RealMoneyProductButtonView>();
            if (buttonView != null)
            {
                buttonView.Initialize(product.productId, storeController);
                _productButtons.Add(buttonView);
            }
        }
    }

    /// <summary>
    /// Очищает список продуктов.
    /// </summary>
    private void ClearProducts()
    {
        foreach (var button in _productButtons)
        {
            if (button != null)
                Destroy(button.gameObject);
        }
        _productButtons.Clear();
    }
}
