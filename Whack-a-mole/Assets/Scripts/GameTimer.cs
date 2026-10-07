using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    [Header("Timer")]
    [SerializeField] private float gameTime = 60f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    private float currentTime;
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
    }

    private void Start()
    {
        currentTime = gameTime;
        isGameOver = false;

        UpdateTimerUI();
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;

            UpdateTimerUI();

            GameOver();
            return;
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text =
                "시간 : " + Mathf.CeilToInt(currentTime);
        }
    }

    private void GameOver()
    {
        isGameOver = true;

        // 게임 결과 처리
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}