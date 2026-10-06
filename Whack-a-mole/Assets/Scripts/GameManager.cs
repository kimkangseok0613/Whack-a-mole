using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Result Panel")]
    [SerializeField] private GameObject resultPanel;

    [Header("Result UI")]
    [SerializeField] private ResultUI resultUI;

    private bool isGameOver;

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

        Time.timeScale = 1f;

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

        // 결과 패널 표시
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        // 현재 점수 저장 및 결과 표시
        if (resultUI != null)
        {
            resultUI.ShowResult();
        }

        // 게임 일시정지
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

        SceneManager.LoadScene("Title");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}