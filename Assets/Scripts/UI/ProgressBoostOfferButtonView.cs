using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// UI компонент для отображения и покупки временного буста за BioGel.
/// </summary>
public class ProgressBoostOfferButtonView : MonoBehaviour
{
    [Header("UI Элементы")]
    [Tooltip("Текст названия буста")]
    public TMP_Text nameText;

    [Tooltip("Текст описания буста")]
    public TMP_Text descriptionText;

    [Tooltip("Текст стоимости буста")]
    public TMP_Text costText;

    [Tooltip("Текст времени действия (если активен)")]
    public TMP_Text timeRemainingText;

    [Tooltip("Кнопка покупки")]
    public Button buyButton;

    [Tooltip("Объект, который показывается если буст активен")]
    public GameObject activeIndicator;

    [Tooltip("Объект, который скрывается если буст не разблокирован")]
    public GameObject lockedOverlay;

    private string _boostId;
    private ProgressBoostOfferDefinition _boost;
    private IdleClickerManager _manager;

    /// <summary>
    /// Инициализирует кнопку буста.
    /// </summary>
    public void Initialize(string boostId, IdleClickerManager manager)
    {
        _boostId = boostId;
        _manager = manager;

        if (_manager == null || _manager.GetEngine() == null)
        {
            Debug.LogError("[ProgressBoostOfferButtonView] IdleClickerManager не найден!");
            return;
        }

        var config = manager.config;
        if (config == null)
        {
            Debug.LogError("[ProgressBoostOfferButtonView] Конфиг не найден!");
            return;
        }

        _boost = config.GetBoostById(boostId);
        if (_boost == null)
        {
            Debug.LogError($"[ProgressBoostOfferButtonView] Буст '{boostId}' не найден в конфиге!");
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
    /// Обновляет UI кнопки буста.
    /// </summary>
    public void UpdateUI()
    {
        if (_manager == null || _boost == null) return;

        // Название и описание
        if (nameText != null)
            nameText.text = _boost.displayName;

        if (descriptionText != null)
            descriptionText.text = _boost.description;

        // Стоимость
        long cost = _manager.GetBoostCost(_boostId);
        if (costText != null)
            costText.text = NumberFormatter.Format(cost);

        // Проверка разблокировки
        bool isUnlocked = _manager.IsBoostUnlocked(_boostId);
        if (lockedOverlay != null)
            lockedOverlay.SetActive(!isUnlocked);

        // Проверка активности
        bool isActive = _manager.IsBoostActive(_boostId);
        if (activeIndicator != null)
            activeIndicator.SetActive(isActive);

        // Время действия
        if (timeRemainingText != null)
        {
            if (isActive)
            {
                // Получаем время окончания из engine (нужно добавить метод для этого)
                // Пока просто показываем что активен
                timeRemainingText.text = "ACTIVE";
            }
            else
            {
                timeRemainingText.text = "";
            }
        }

        // Проверка возможности покупки
        bool canAfford = _manager.GetBioGel() >= cost;
        if (buyButton != null)
        {
            buyButton.interactable = isUnlocked && canAfford && !isActive;
        }
    }

    private void OnBuyClicked()
    {
        if (_manager == null) return;
        bool success = _manager.BuyBoost(_boostId);
        if (success)
        {
            UpdateUI();
        }
    }
}
