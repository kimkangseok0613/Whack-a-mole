using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score")]
    [SerializeField] private int score = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

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

        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;

        // 점수가 0점 밑으로 내려가지 않도록 설정
        score = Mathf.Max(0, score);

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE : " + score;
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
}