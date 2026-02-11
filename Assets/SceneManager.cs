using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Менеджер переключения между сценами Space Farm.
/// Переключает между сценой фермы и сценой терраформирования.
/// </summary>
public class SpaceFarmSceneManager : MonoBehaviour
{
    [Header("Названия сцен")]
    [Tooltip("Название сцены фермы (где собирается BioGel).")]
    public string farmSceneName = "FarmScene";

    [Tooltip("Название сцены терраформирования (новая планета-база).")]
    public string terraformSceneName = "TerraformScene";

    [Header("UI Кнопки переключения (необязательно)")]
    [Tooltip("Кнопка перехода на ферму. Если не задана, будет найдена по имени FarmButton.")]
    public UnityEngine.UI.Button farmButton;

    [Tooltip("Кнопка перехода на терраформирование. Если не задана, будет найдена по имени TerraformButton.")]
    public UnityEngine.UI.Button terraformButton;

    private void Start()
    {
        AutoWireButtons();
    }

    private void AutoWireButtons()
    {
        if (farmButton == null)
        {
            var go = GameObject.Find("FarmButton");
            if (go != null)
            {
                farmButton = go.GetComponent<UnityEngine.UI.Button>();
                if (farmButton != null)
                    farmButton.onClick.AddListener(LoadFarmScene);
            }
        }
        else
        {
            farmButton.onClick.AddListener(LoadFarmScene);
        }

        if (terraformButton == null)
        {
            var go = GameObject.Find("TerraformButton");
            if (go != null)
            {
                terraformButton = go.GetComponent<UnityEngine.UI.Button>();
                if (terraformButton != null)
                    terraformButton.onClick.AddListener(LoadTerraformScene);
            }
        }
        else
        {
            terraformButton.onClick.AddListener(LoadTerraformScene);
        }
    }

    /// <summary>
    /// Загружает сцену фермы.
    /// </summary>
    public void LoadFarmScene()
    {
        SceneManager.LoadScene(farmSceneName);
    }

    /// <summary>
    /// Загружает сцену терраформирования.
    /// </summary>
    public void LoadTerraformScene()
    {
        SceneManager.LoadScene(terraformSceneName);
    }
}
