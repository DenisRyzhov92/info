using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

/// <summary>
/// MonoBehaviour-обёртка для IdleClickerEngine.
/// Управляет UI, сохранениями, оффлайн-доходом и интеграцией с Unity.
/// </summary>
public class IdleClickerManager : MonoBehaviour
{
    private static IdleClickerManager _instance;

    public static IdleClickerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<IdleClickerManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("IdleClickerManager");
                    _instance = go.AddComponent<IdleClickerManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [Header("Конфиг")]
    [Tooltip("ScriptableObject конфиг со всеми апгрейдами и бустами")]
    public IdleClickerConfig config;

    [Header("UI - Ресурсы")]
    [Tooltip("Текст для отображения BioGel")]
    public TMP_Text bioGelText;

    [Tooltip("Текст для отображения пассивного дохода (BioGel/s)")]
    public TMP_Text perSecondText;

    [Tooltip("Текст для отображения lifetime BioGel (необязательно)")]
    public TMP_Text lifetimeBioGelText;

    [Header("UI - Кнопки")]
    [Tooltip("Кнопка тапа (ручной сбор). Если не задана, будет найдена по имени ClickButton")]
    public Button tapButton;

    [Header("Оффлайн-доход")]
    [Tooltip("Максимальное количество часов оффлайн-дохода")]
    public int maxOfflineHours = 24;

    [Tooltip("Показывать ли уведомление об оффлайн-доходе при загрузке")]
    public bool showOfflineIncomeNotification = true;

    // Ядро экономики
    private IdleClickerEngine _engine;

    // Таймер для пассивного дохода
    private float _passiveTimer;

    // Таймер для обновления бустов
    private float _boostUpdateTimer;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        InitializeEngine();
        LoadProgress();
        AutoWireUI();
        UpdateUI();
    }

    private void Update()
    {
        if (_engine == null) return;

        // Обновляем активные бусты раз в секунду
        _boostUpdateTimer += Time.deltaTime;
        if (_boostUpdateTimer >= 1f)
        {
            _boostUpdateTimer = 0f;
            _engine.UpdateActiveBoosts();
        }

        // Пассивный доход
        float idleIncome = _engine.GetIdleIncome();
        if (idleIncome > 0f)
        {
            _passiveTimer += Time.deltaTime;
            if (_passiveTimer >= 1f)
            {
                int wholeSeconds = Mathf.FloorToInt(_passiveTimer);
                _passiveTimer -= wholeSeconds;
                _engine.AddBioGel((long)(idleIncome * wholeSeconds));
                SaveProgress();
            }
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveProgress();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveProgress();
        }
    }

    /// <summary>
    /// Инициализирует ядро экономики.
    /// </summary>
    private void InitializeEngine()
    {
        if (config == null)
        {
            Debug.LogError("[IdleClickerManager] Конфиг не назначен! Создайте IdleClickerConfig через меню Space Farm/Create Default Config");
            return;
        }

        _engine = new IdleClickerEngine(config);
        _engine.OnBioGelChanged += UpdateUI;
        _engine.OnUpgradePurchased += OnUpgradePurchased;
        _engine.OnBoostActivated += OnBoostActivated;
    }

    /// <summary>
    /// Тап по полю (ручной сбор BioGel).
    /// </summary>
    public void OnTapField()
    {
        if (_engine == null) return;
        _engine.Tap();
        SaveProgress();
    }

    /// <summary>
    /// Покупает апгрейд по ID.
    /// </summary>
    public bool BuyUpgrade(string upgradeId)
    {
        if (_engine == null) return false;
        bool success = _engine.BuyUpgrade(upgradeId);
        if (success)
        {
            SaveProgress();
        }
        return success;
    }

    /// <summary>
    /// Покупает временный буст за BioGel.
    /// </summary>
    public bool BuyBoost(string boostId)
    {
        if (_engine == null) return false;
        bool success = _engine.BuyBoost(boostId);
        if (success)
        {
            SaveProgress();
        }
        return success;
    }

    /// <summary>
    /// Получает текущее количество BioGel.
    /// </summary>
    public long GetBioGel()
    {
        return _engine?.BioGel ?? 0;
    }

    /// <summary>
    /// Получает lifetime BioGel.
    /// </summary>
    public long GetLifetimeBioGel()
    {
        return _engine?.LifetimeBioGel ?? 0;
    }

    /// <summary>
    /// Получает уровень апгрейда.
    /// </summary>
    public int GetUpgradeLevel(string upgradeId)
    {
        return _engine?.GetUpgradeLevel(upgradeId) ?? 0;
    }

    /// <summary>
    /// Проверяет, разблокирован ли апгрейд.
    /// </summary>
    public bool IsUpgradeUnlocked(string upgradeId)
    {
        return _engine?.IsUpgradeUnlocked(upgradeId) ?? false;
    }

    /// <summary>
    /// Проверяет, разблокирован ли буст.
    /// </summary>
    public bool IsBoostUnlocked(string boostId)
    {
        return _engine?.IsBoostUnlocked(boostId) ?? false;
    }

    /// <summary>
    /// Проверяет, активен ли буст.
    /// </summary>
    public bool IsBoostActive(string boostId)
    {
        return _engine?.IsBoostActive(boostId) ?? false;
    }

    /// <summary>
    /// Получает стоимость апгрейда для следующего уровня.
    /// </summary>
    public long GetUpgradeCost(string upgradeId)
    {
        if (_engine == null || config == null) return 0;
        var upgrade = config.GetUpgradeById(upgradeId);
        if (upgrade == null) return 0;
        int currentLevel = _engine.GetUpgradeLevel(upgradeId);
        return upgrade.GetCostForLevel(currentLevel);
    }

    /// <summary>
    /// Получает стоимость буста.
    /// </summary>
    public long GetBoostCost(string boostId)
    {
        if (config == null) return 0;
        var boost = config.GetBoostById(boostId);
        return boost?.cost ?? 0;
    }

    /// <summary>
    /// Получает ядро экономики (для прямого доступа из UI).
    /// </summary>
    public IdleClickerEngine GetEngine()
    {
        return _engine;
    }

    /// <summary>
    /// Обновляет UI.
    /// </summary>
    private void UpdateUI()
    {
        if (_engine == null) return;

        if (bioGelText != null)
            bioGelText.text = "BioGel: " + NumberFormatter.Format(_engine.BioGel);

        if (perSecondText != null)
        {
            float idleIncome = _engine.GetIdleIncome();
            perSecondText.text = idleIncome.ToString("0.0") + " BioGel/s";
        }

        if (lifetimeBioGelText != null)
            lifetimeBioGelText.text = "Lifetime: " + NumberFormatter.Format(_engine.LifetimeBioGel);
    }

    /// <summary>
    /// Автоматически находит UI элементы по имени.
    /// </summary>
    private void AutoWireUI()
    {
        if (bioGelText == null)
        {
            var go = GameObject.Find("BioGelText");
            if (go != null)
                bioGelText = go.GetComponent<TMP_Text>();
        }

        if (perSecondText == null)
        {
            var go = GameObject.Find("PerSecondText");
            if (go != null)
                perSecondText = go.GetComponent<TMP_Text>();
        }

        if (tapButton == null)
        {
            var go = GameObject.Find("ClickButton");
            if (go != null)
            {
                tapButton = go.GetComponent<Button>();
                if (tapButton != null)
                    tapButton.onClick.AddListener(OnTapField);
            }
        }
    }

    /// <summary>
    /// Сохраняет прогресс в JSON файл.
    /// </summary>
    public void SaveProgress()
    {
        if (_engine == null) return;
        var saveData = _engine.CreateSaveData();
        IdleSaveStorage.Save(saveData);
    }

    /// <summary>
    /// Загружает прогресс из JSON файла.
    /// </summary>
    public void LoadProgress()
    {
        if (_engine == null)
        {
            InitializeEngine();
            if (_engine == null) return;
        }

        var saveData = IdleSaveStorage.Load();
        _engine.LoadFromSave(saveData);

        // Проверяем оффлайн-доход
        if (saveData.lastSaveTimestamp > 0)
        {
            long offlineIncome = IdleSaveStorage.CalculateOfflineIncome(
                saveData.lastSaveTimestamp,
                saveData.bioGelPerSecond,
                maxOfflineHours
            );

            if (offlineIncome > 0)
            {
                _engine.AddBioGel(offlineIncome);
                if (showOfflineIncomeNotification)
                {
                    Debug.Log($"[IdleClickerManager] Оффлайн-доход: {NumberFormatter.Format(offlineIncome)} BioGel");
                }
            }
        }

        UpdateUI();
    }

    private void OnUpgradePurchased()
    {
        UpdateUI();
    }

    private void OnBoostActivated()
    {
        UpdateUI();
    }
}
