using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private void Update()
    {
        // ESC 키를 누르면 일시정지 / 재개
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    // ESC / 버튼으로 일시정지 상태 전환
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // 일시정지
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

    // 게임 재개
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

    // 게임 재시작
    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    // 타이틀로 이동
    public void GoToTitle()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(titleSceneName);
    }

    // 점수 UI 업데이트
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

    // 일시정지 패널 표시 / 숨김
    private void SetPausePanel(bool active)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(active);
        }
    }
}