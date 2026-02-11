using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Менеджер idle‑кликера Space Farm для CosmoFarm.
/// Управляет BioGel, тапом (ручной сбор) и idle-доходом (дроны).
/// Синглтон - сохраняет прогресс между сценами.
/// </summary>
public class IdleFarmManager : MonoBehaviour
{
    private static IdleFarmManager _instance;

    public static IdleFarmManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<IdleFarmManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<IdleFarmManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    [Header("Ресурсы")]
    [Tooltip("Текущее количество BioGel (основной ресурс космофермы).")]
    public long bioGel = 0;

    [Tooltip("Сколько BioGel даёт один тап (ручной сбор в куполе).")]
    public long bioGelPerTap = 1;

    [Tooltip("Сколько BioGel в секунду даёт пассивный доход (автоматическая добыча дронами).")]
    public float bioGelPerSecond = 0f;

    [Header("Прокачка клика")]
    [Tooltip("Текущий уровень улучшения клика.")]
    public int clickUpgradeLevel = 0;

    [Tooltip("Базовая стоимость первого улучшения клика.")]
    public long clickUpgradeBaseCost = 10;

    [Tooltip("Во сколько раз растёт цена каждого следующего улучшения.")]
    public float clickUpgradeCostMultiplier = 1.7f;

    [Header("UI")]
    [Tooltip("Текст, в который выводится текущее количество BioGel.")]
    public TMP_Text bioGelText;

    [Tooltip("Текст, где показывается пассивный доход в секунду (BioGel/s) (необязательно).")]
    public TMP_Text perSecondText;

    [Tooltip("Текст с стоимостью улучшения тапа (необязательно).")]
    public TMP_Text tapUpgradeCostText;

    [Header("UI Кнопки (необязательно)")]
    [Tooltip("Кнопка клика по полю. Если не задана, будет найдена по имени ClickButton.")]
    public Button clickButton;

    [Tooltip("Кнопка апгрейда клика. Если не задана, будет найдена по имени UpgradeButton.")]
    public Button upgradeButton;

    // Счётчик времени для пассивного дохода
    private float _passiveTimer;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        AutoWireUI();
        UpdateUI();
    }

    /// <summary>
    /// Тап по полю фермы (ручной сбор BioGel в куполе).
    /// </summary>
    public void OnTapField()
    {
        bioGel += bioGelPerTap;
        UpdateUI();
        SaveProgress();
    }

    /// <summary>
    /// Покупка улучшения тапа (Manual Harvest Protocol).
    /// </summary>
    public void OnBuyTapUpgrade()
    {
        long cost = GetTapUpgradeCost();
        if (bioGel < cost)
            return; // Не хватает BioGel

        bioGel -= cost;
        clickUpgradeLevel++;
        bioGelPerTap++;

        UpdateUI();
        SaveProgress();
    }

    private void Update()
    {
        if (bioGelPerSecond <= 0f)
            return;

        _passiveTimer += Time.deltaTime;

        if (_passiveTimer >= 1f)
        {
            // Сколько целых секунд прошло
            int wholeSeconds = Mathf.FloorToInt(_passiveTimer);
            _passiveTimer -= wholeSeconds;

            // Добавляем пассивный доход (автоматическая добыча дронами)
            bioGel += (long)(bioGelPerSecond * wholeSeconds);
            UpdateUI();
            SaveProgress();
        }
    }

    private void UpdateUI()
    {
        if (bioGelText != null)
            bioGelText.text = "BioGel: " + FormatNumber(bioGel);

        if (perSecondText != null)
            perSecondText.text = bioGelPerSecond.ToString("0.0") + " BioGel/s";

        if (tapUpgradeCostText != null)
            tapUpgradeCostText.text = "Upgrade: " + FormatNumber(GetTapUpgradeCost());
    }

    private string FormatNumber(long value)
    {
        if (value >= 1000000)
            return (value / 1000000f).ToString("0.0") + "M";
        if (value >= 1000)
            return (value / 1000f).ToString("0.0") + "K";
        return value.ToString();
    }

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

        if (tapUpgradeCostText == null)
        {
            var go = GameObject.Find("TapUpgradeCostText");
            if (go != null)
                tapUpgradeCostText = go.GetComponent<TMP_Text>();
        }

        if (clickButton == null)
        {
            var go = GameObject.Find("ClickButton");
            if (go != null)
            {
                clickButton = go.GetComponent<Button>();
                if (clickButton != null)
                    clickButton.onClick.AddListener(OnTapField);
            }
        }

        if (upgradeButton == null)
        {
            var go = GameObject.Find("UpgradeButton");
            if (go != null)
            {
                upgradeButton = go.GetComponent<Button>();
                if (upgradeButton != null)
                    upgradeButton.onClick.AddListener(OnBuyTapUpgrade);
            }
        }
    }

    private long GetTapUpgradeCost()
    {
        // cost = baseCost * multiplier^level
        float cost = clickUpgradeBaseCost * Mathf.Pow(clickUpgradeCostMultiplier, clickUpgradeLevel);
        return Mathf.CeilToInt(cost);
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetString("SF_BioGel", bioGel.ToString());
        PlayerPrefs.SetInt("SF_TapLevel", clickUpgradeLevel);
        PlayerPrefs.SetString("SF_BioGelPerTap", bioGelPerTap.ToString());
        PlayerPrefs.SetFloat("SF_BioGelPerSecond", bioGelPerSecond);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        if (!PlayerPrefs.HasKey("SF_BioGel"))
            return;

        long.TryParse(PlayerPrefs.GetString("SF_BioGel", "0"), out bioGel);
        long.TryParse(PlayerPrefs.GetString("SF_BioGelPerTap", "1"), out bioGelPerTap);
        bioGelPerSecond = PlayerPrefs.GetFloat("SF_BioGelPerSecond", bioGelPerSecond);
        clickUpgradeLevel = PlayerPrefs.GetInt("SF_TapLevel", clickUpgradeLevel);
    }
}

