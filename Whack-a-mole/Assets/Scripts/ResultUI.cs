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

        // 현재 게임 모드의 최고 점수 저장
        ScoreManager.Instance.SaveHighScore();

        // 현재 게임 모드의 최고 점수 가져오기
        int highScore = ScoreManager.Instance.GetHighScore();

        if (scoreText != null)
        {
            scoreText.text = "점수 : " + currentScore;
        }

        if (highScoreText != null)
        {
            highScoreText.text = "최고 점수 : " + highScore;
        }
    }
}