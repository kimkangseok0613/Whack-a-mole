using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        score = 0;
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        score = Mathf.Max(0, score);

        SaveHighScore();
        UpdateScoreUI();
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(GetHighScoreKey(), 0);
    }

    public void SaveHighScore()
    {
        int currentHighScore = GetHighScore();

        if (score <= currentHighScore)
        {
            return;
        }

        PlayerPrefs.SetInt(GetHighScoreKey(), score);
        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText == null)
        {
            return;
        }

        scoreText.text = "Á¡¼ö : " + score;
    }

    private string GetHighScoreKey()
    {
        return gameMode == GameMode.Normal
            ? NormalHighScoreKey
            : HardHighScoreKey;
    }
}