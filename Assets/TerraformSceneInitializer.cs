using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Автоматически создаёт и настраивает все объекты для сцены TerraformScene при запуске.
/// Просто добавь этот скрипт на любой объект в сцене - он всё настроит сам.
/// </summary>
public class TerraformSceneInitializer : MonoBehaviour
{
    [Header("Автоматическая настройка")]
    [Tooltip("Если включено, скрипт автоматически создаст все объекты при старте.")]
    public bool autoSetupOnStart = true;

    private void Start()
    {
        if (autoSetupOnStart)
        {
            SetupTerraformScene();
        }
    }

    [ContextMenu("Setup Terraform Scene")]
    public void SetupTerraformScene()
    {
        Debug.Log("Начинаю автоматическую настройку TerraformScene...");

        // 1. Создаём или находим SceneManager
        SpaceFarmSceneManager sceneManager = FindObjectOfType<SpaceFarmSceneManager>();
        if (sceneManager == null)
        {
            GameObject smObj = new GameObject("SceneManager");
            sceneManager = smObj.AddComponent<SpaceFarmSceneManager>();
            sceneManager.farmSceneName = "FarmScene";
            sceneManager.terraformSceneName = "TerraformScene";
            Debug.Log("Создан SceneManager");
        }

        // 2. Создаём или находим Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // Создаём EventSystem если его нет
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystemObj = new GameObject("EventSystem");
                eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            Debug.Log("Создан Canvas");
        }

        // 3. Создаём UI элементы
        CreateUIText(canvas.transform, "BioGelText", "BioGel: 0", 36, new Vector2(-400, 400));
        CreateUIText(canvas.transform, "PerSecondText", "0.0 BioGel/s", 24, new Vector2(-400, 350));
        CreateUIText(canvas.transform, "TitleText", "Terraforming New Planet", 48, new Vector2(0, 300));

        // 4. Создаём кнопку возврата на ферму
        CreateButton(canvas.transform, "FarmButton", "Back to Farm", new Vector2(-400, 250), new Vector2(200, 50));

        Debug.Log("Автоматическая настройка TerraformScene завершена!");
    }

    private void CreateUIText(Transform parent, string name, string text, int fontSize, Vector2 position)
    {
        GameObject existing = GameObject.Find(name);
        if (existing != null)
        {
            Debug.Log($"UI элемент {name} уже существует, пропускаю");
            return;
        }

        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);

        TextMeshProUGUI tmpText = textObj.AddComponent<TextMeshProUGUI>();
        tmpText.text = text;
        tmpText.fontSize = fontSize;
        tmpText.alignment = TextAlignmentOptions.Left;

        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(400, 50);

        Debug.Log($"Создан UI текст: {name}");
    }

    private void CreateButton(Transform parent, string name, string buttonText, Vector2 position, Vector2 size)
    {
        GameObject existing = GameObject.Find(name);
        if (existing != null)
        {
            Debug.Log($"Кнопка {name} уже существует, пропускаю");
            return;
        }

        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);

        Image image = buttonObj.AddComponent<Image>();
        image.color = new Color(0.2f, 0.6f, 0.9f, 1f); // Синий цвет

        Button button = buttonObj.AddComponent<Button>();

        RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;

        // Создаём дочерний текст
        GameObject textObj = new GameObject("Text (TMP)");
        textObj.transform.SetParent(buttonObj.transform, false);

        TextMeshProUGUI tmpText = textObj.AddComponent<TextMeshProUGUI>();
        tmpText.text = buttonText;
        tmpText.fontSize = 24;
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.color = Color.white;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        Debug.Log($"Создана кнопка: {name}");
    }
}
