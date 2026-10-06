using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [Header("Result Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private const string HighScoreKey = "HighScore";

    public void ShowResult()
    {
        int currentScore = 0;

        if (ScoreManager.Instance != null)
        {
            currentScore =
                ScoreManager.Instance.GetScore();
        }

        int highScore =
            PlayerPrefs.GetInt(HighScoreKey, 0);

        // 최고 점수 갱신
        if (currentScore > highScore)
        {
            highScore = currentScore;

            PlayerPrefs.SetInt(
                HighScoreKey,
                highScore
            );

            PlayerPrefs.Save();
        }

        if (scoreText != null)
        {
            scoreText.text =
                "SCORE : " + currentScore;
        }

        if (highScoreText != null)
        {
            highScoreText.text =
                "HIGH SCORE : " + highScore;
        }
    }
}