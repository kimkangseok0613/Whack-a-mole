using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [Header("Difficulty Panel")]
    [SerializeField] private GameObject difficultyPanel;

    [Header("Scene Names")]
    [SerializeField] private string normalSceneName = "Normal";
    [SerializeField] private string hardSceneName = "Hard";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        CloseDifficultyPanel();
    }

    public void OpenDifficultyPanel()
    {
        SetDifficultyPanel(true);
    }

    public void CloseDifficultyPanel()
    {
        SetDifficultyPanel(false);
    }

    public void SelectNormal()
    {
        LoadScene(normalSceneName);
    }

    public void SelectHard()
    {
        LoadScene(hardSceneName);
    }

    private void SetDifficultyPanel(bool active)
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(active);
        }
    }

    private void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}