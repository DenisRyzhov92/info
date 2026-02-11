using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI компонент для отображения и покупки апгрейда.
/// Привязывается к кнопке апгрейда в списке.
/// </summary>
public class UpgradeButtonView : MonoBehaviour
{
    [Header("UI Элементы")]
    [Tooltip("Текст названия апгрейда")]
    public TMP_Text nameText;

    [Tooltip("Текст описания апгрейда")]
    public TMP_Text descriptionText;

    [Tooltip("Текст уровня апгрейда")]
    public TMP_Text levelText;

    [Tooltip("Текст стоимости апгрейда")]
    public TMP_Text costText;

    [Tooltip("Кнопка покупки")]
    public Button buyButton;

    [Tooltip("Объект, который скрывается если апгрейд не разблокирован")]
    public GameObject lockedOverlay;

    private string _upgradeId;
    private UpgradeDefinition _upgrade;
    private IdleClickerManager _manager;

    /// <summary>
    /// Инициализирует кнопку апгрейда.
    /// </summary>
    public void Initialize(string upgradeId, IdleClickerManager manager)
    {
        _upgradeId = upgradeId;
        _manager = manager;

        if (_manager == null || _manager.GetEngine() == null)
        {
            Debug.LogError("[UpgradeButtonView] IdleClickerManager не найден!");
            return;
        }

        var config = manager.config;
        if (config == null)
        {
            Debug.LogError("[UpgradeButtonView] Конфиг не найден!");
            return;
        }

        _upgrade = config.GetUpgradeById(upgradeId);
        if (_upgrade == null)
        {
            Debug.LogError($"[UpgradeButtonView] Апгрейд '{upgradeId}' не найден в конфиге!");
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
    /// Обновляет UI кнопки апгрейда.
    /// </summary>
    public void UpdateUI()
    {
        if (_manager == null || _upgrade == null) return;

        // Название и описание
        if (nameText != null)
            nameText.text = _upgrade.displayName;

        if (descriptionText != null)
            descriptionText.text = _upgrade.description;

        // Уровень
        int currentLevel = _manager.GetUpgradeLevel(_upgradeId);
        if (levelText != null)
            levelText.text = $"Level {currentLevel}";

        // Стоимость
        long cost = _manager.GetUpgradeCost(_upgradeId);
        if (costText != null)
            costText.text = NumberFormatter.Format(cost);

        // Проверка разблокировки
        bool isUnlocked = _manager.IsUpgradeUnlocked(_upgradeId);
        if (lockedOverlay != null)
            lockedOverlay.SetActive(!isUnlocked);

        // Проверка возможности покупки
        bool canAfford = _manager.GetBioGel() >= cost;
        if (buyButton != null)
        {
            buyButton.interactable = isUnlocked && canAfford;
        }
    }

    private void OnBuyClicked()
    {
        if (_manager == null) return;
        bool success = _manager.BuyUpgrade(_upgradeId);
        if (success)
        {
            UpdateUI();
        }
    }
}
