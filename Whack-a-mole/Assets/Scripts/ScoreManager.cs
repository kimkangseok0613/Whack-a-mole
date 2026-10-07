using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public enum GameMode
    {
        Normal,
        Hard
    }

    [Header("Game Mode")]
    [SerializeField] private GameMode gameMode;

    [Header("Score")]
    [SerializeField] private int score = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private const string NormalHighScoreKey = "NormalHighScore";
    private const string HardHighScoreKey = "HardHighScore";

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

        score = 0;

        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;

        // 점수가 0보다 작아지지 않도록 설정
        score = Mathf.Max(0, score);

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "점수 : " + score;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;

        UpdateScoreUI();
    }

    public int GetHighScore()
    {
        string key = GetHighScoreKey();

        return PlayerPrefs.GetInt(key, 0);
    }

    public void SaveHighScore()
    {
        string key = GetHighScoreKey();

        int highScore = PlayerPrefs.GetInt(key, 0);

        if (score > highScore)
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
        }
    }

    private string GetHighScoreKey()
    {
        if (gameMode == GameMode.Normal)
        {
            return NormalHighScoreKey;
        }

        return HardHighScoreKey;
    }
}