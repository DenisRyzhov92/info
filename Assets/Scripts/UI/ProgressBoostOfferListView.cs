using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI компонент для отображения списка всех временных бустов за BioGel.
/// </summary>
public class ProgressBoostOfferListView : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("IdleClickerManager (если не задан, будет найден автоматически)")]
    public IdleClickerManager manager;

    [Tooltip("Prefab кнопки буста")]
    public GameObject boostButtonPrefab;

    [Tooltip("Родительский объект для размещения кнопок (обычно Content ScrollView)")]
    public Transform contentRoot;

    private List<ProgressBoostOfferButtonView> _boostButtons = new List<ProgressBoostOfferButtonView>();

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Инициализирует список бустов.
    /// </summary>
    public void Initialize()
    {
        if (manager == null)
            manager = IdleClickerManager.Instance;

        if (manager == null || manager.config == null)
        {
            Debug.LogError("[ProgressBoostOfferListView] IdleClickerManager или конфиг не найден!");
            return;
        }

        if (boostButtonPrefab == null)
        {
            Debug.LogError("[ProgressBoostOfferListView] Prefab кнопки буста не назначен!");
            return;
        }

        if (contentRoot == null)
        {
            Debug.LogError("[ProgressBoostOfferListView] Content Root не назначен!");
            return;
        }

        // Очищаем старые кнопки
        ClearBoosts();

        // Создаём кнопки для каждого буста
        foreach (var boost in manager.config.boostOffers)
        {
            GameObject buttonObj = Instantiate(boostButtonPrefab, contentRoot);
            ProgressBoostOfferButtonView buttonView = buttonObj.GetComponent<ProgressBoostOfferButtonView>();
            if (buttonView != null)
            {
                buttonView.Initialize(boost.id, manager);
                _boostButtons.Add(buttonView);
            }
        }
    }

    /// <summary>
    /// Обновляет все кнопки бустов.
    /// </summary>
    public void RefreshBoosts()
    {
        foreach (var button in _boostButtons)
        {
            if (button != null)
                button.UpdateUI();
        }
    }

    /// <summary>
    /// Очищает список бустов.
    /// </summary>
    private void ClearBoosts()
    {
        foreach (var button in _boostButtons)
        {
            if (button != null)
                Destroy(button.gameObject);
        }
        _boostButtons.Clear();
    }

    private void Update()
    {
        // Обновляем UI раз в секунду для отображения актуального состояния бустов
        if (Time.frameCount % 60 == 0)
        {
            RefreshBoosts();
        }
    }
}
