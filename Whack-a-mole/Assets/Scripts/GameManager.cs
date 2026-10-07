using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Result Panel")]
    [SerializeField] private GameObject resultPanel;

    [Header("Result UI")]
    [SerializeField] private ResultUI resultUI;

    [Header("Scenes")]
    [SerializeField] private string titleSceneName = "Title";

    private bool isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Time.timeScale = 1f;
        isGameOver = false;

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        if (resultUI != null)
        {
            resultUI.ShowResult();
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(titleSceneName);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}