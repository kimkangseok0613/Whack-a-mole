using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [Header("Result Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    public void ShowResult()
    {
        if (ScoreManager.Instance == null)
        {
            return;
        }

        int currentScore = ScoreManager.Instance.GetScore();
        int highScore = ScoreManager.Instance.GetHighScore();

        UpdateScoreText(currentScore);
        UpdateHighScoreText(highScore);
    }

    private void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "점수 : " + score;
        }
    }

    private void UpdateHighScoreText(int highScore)
    {
        if (highScoreText != null)
        {
            highScoreText.text = "최고 점수 : " + highScore;
        }
    }
}