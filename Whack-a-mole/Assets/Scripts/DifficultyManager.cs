using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    [Header("Difficulty Panel")]
    [SerializeField] private GameObject difficultyPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
        }
    }

    // 난이도 선택창 열기
    public void OpenDifficultyPanel()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(true);
        }
    }

    // 난이도 선택창 닫기
    public void CloseDifficultyPanel()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
        }
    }

    // Normal 선택
    public void SelectNormal()
    {
        SceneManager.LoadScene("Normal");
    }

    // Hard 선택
    public void SelectHard()
    {
        SceneManager.LoadScene("Hard");
    }
}