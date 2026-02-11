using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI компонент для отображения списка всех апгрейдов.
/// Автоматически создаёт кнопки апгрейдов из конфига.
/// </summary>
public class UpgradeListView : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("IdleClickerManager (если не задан, будет найден автоматически)")]
    public IdleClickerManager manager;

    [Tooltip("Prefab кнопки апгрейда")]
    public GameObject upgradeButtonPrefab;

    [Tooltip("Родительский объект для размещения кнопок (обычно Content ScrollView)")]
    public Transform contentRoot;

    private List<UpgradeButtonView> _upgradeButtons = new List<UpgradeButtonView>();

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Инициализирует список апгрейдов.
    /// </summary>
    public void Initialize()
    {
        if (manager == null)
            manager = IdleClickerManager.Instance;

        if (manager == null || manager.config == null)
        {
            Debug.LogError("[UpgradeListView] IdleClickerManager или конфиг не найден!");
            return;
        }

        if (upgradeButtonPrefab == null)
        {
            Debug.LogError("[UpgradeListView] Prefab кнопки апгрейда не назначен!");
            return;
        }

        if (contentRoot == null)
        {
            Debug.LogError("[UpgradeListView] Content Root не назначен!");
            return;
        }

        // Очищаем старые кнопки
        ClearUpgrades();

        // Создаём кнопки для каждого апгрейда
        foreach (var upgrade in manager.config.upgrades)
        {
            GameObject buttonObj = Instantiate(upgradeButtonPrefab, contentRoot);
            UpgradeButtonView buttonView = buttonObj.GetComponent<UpgradeButtonView>();
            if (buttonView != null)
            {
                buttonView.Initialize(upgrade.id, manager);
                _upgradeButtons.Add(buttonView);
            }
        }
    }

    /// <summary>
    /// Обновляет все кнопки апгрейдов.
    /// </summary>
    public void RefreshUpgrades()
    {
        foreach (var button in _upgradeButtons)
        {
            if (button != null)
                button.UpdateUI();
        }
    }

    /// <summary>
    /// Очищает список апгрейдов.
    /// </summary>
    private void ClearUpgrades()
    {
        foreach (var button in _upgradeButtons)
        {
            if (button != null)
                Destroy(button.gameObject);
        }
        _upgradeButtons.Clear();
    }

    private void Update()
    {
        // Обновляем UI раз в секунду для отображения актуальных цен
        if (Time.frameCount % 60 == 0)
        {
            RefreshUpgrades();
        }
    }
}
