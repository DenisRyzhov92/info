using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI компонент для отображения и покупки IAP продукта.
/// </summary>
public class RealMoneyProductButtonView : MonoBehaviour
{
    [Header("UI Элементы")]
    [Tooltip("Текст названия продукта")]
    public TMP_Text nameText;

    [Tooltip("Текст описания продукта")]
    public TMP_Text descriptionText;

    [Tooltip("Текст цены продукта")]
    public TMP_Text priceText;

    [Tooltip("Кнопка покупки")]
    public Button buyButton;

    private string _productId;
    private RealMoneyProductDefinition _product;
    private RealMoneyStoreController _storeController;

    /// <summary>
    /// Инициализирует кнопку продукта.
    /// </summary>
    public void Initialize(string productId, RealMoneyStoreController storeController)
    {
        _productId = productId;
        _storeController = storeController;

        if (_storeController == null || _storeController.manager == null)
        {
            Debug.LogError("[RealMoneyProductButtonView] RealMoneyStoreController не найден!");
            return;
        }

        var config = _storeController.manager.config;
        if (config == null)
        {
            Debug.LogError("[RealMoneyProductButtonView] Конфиг не найден!");
            return;
        }

        _product = config.GetProductById(productId);
        if (_product == null)
        {
            Debug.LogError($"[RealMoneyProductButtonView] Продукт '{productId}' не найден в конфиге!");
            return;
        }

        // Настраиваем кнопку
        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyClicked);
        }

        UpdateUI();
    }

    /// <summary>
    /// Обновляет UI кнопки продукта.
    /// </summary>
    public void UpdateUI()
    {
        if (_product == null) return;

        // Название и описание
        if (nameText != null)
            nameText.text = _product.displayName;

        if (descriptionText != null)
            descriptionText.text = _product.description;

        // Цена (в реальной игре получается из магазина, здесь показываем placeholder)
        if (priceText != null)
        {
            // В реальной игре здесь должна быть цена из магазина
            priceText.text = "Buy";
        }
    }

    private void OnBuyClicked()
    {
        if (_storeController == null) return;
        _storeController.PurchaseProduct(_productId);
    }
}
