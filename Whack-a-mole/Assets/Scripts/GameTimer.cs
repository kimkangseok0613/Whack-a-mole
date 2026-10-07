using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    [Header("Timer")]
    [SerializeField] private float gameTime = 60f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    private float currentTime;
    private bool isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
            isGameOver = true;

            UpdateTimerUI();
            TriggerGameOver();

            return;
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text =
                "½Ã°£ : " + Mathf.CeilToInt(currentTime);
        }
    }

    private void TriggerGameOver()
    {
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