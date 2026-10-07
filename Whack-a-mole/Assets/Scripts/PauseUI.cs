using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Scene")]
    [SerializeField] private string titleSceneName = "Title";

    private bool isPaused;

    private void Awake()
    {
        Time.timeScale = 1f;
        SetPausePanel(false);
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            return;
        }

        isPaused = true;

        UpdateScoreUI();
        SetPausePanel(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!isPaused)
        {
            return;
        }

        isPaused = false;

        SetPausePanel(false);

        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(titleSceneName);
    }

    private void UpdateScoreUI()
    {
        if (ScoreManager.Instance == null)
        {
            return;
        }

        int currentScore =
            ScoreManager.Instance.GetScore();

        int highScore =
            ScoreManager.Instance.GetHighScore();

        if (scoreText != null)
        {
            scoreText.text =
                "현재 점수 : " + currentScore;
        }

        if (highScoreText != null)
        {
            highScoreText.text =
                "최고 점수 : " + highScore;
        }
    }

    private void SetPausePanel(bool active)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(active);
        }
    }
}