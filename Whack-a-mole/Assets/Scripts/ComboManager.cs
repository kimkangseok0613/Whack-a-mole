using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    [Header("Combo")]
    [SerializeField] private int combo = 0;

    [SerializeField]
    private int comboStep = 10;

    [SerializeField]
    private int maxMultiplier = 5;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI multiplierText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ResetCombo();
    }

    public void RegisterNormalHit()
    {
        AddCombo();
    }

    public void RegisterBadHit()
    {
        ResetCombo();
    }

    public void AddCombo()
    {
        combo++;
        UpdateComboUI();
    }

    public void ResetCombo()
    {
        combo = 0;
        UpdateComboUI();
    }

    public int GetCombo()
    {
        return combo;
    }

    public int GetMultiplier()
    {
        if (comboStep <= 0)
        {
            return 1;
        }

        int multiplier = (combo / comboStep) + 1;

        return Mathf.Min(multiplier, maxMultiplier);
    }

    private void UpdateComboUI()
    {
        if (comboText != null)
        {
            comboText.text = "ÄÞº¸ : " + combo;
        }

        if (multiplierText != null)
        {
            multiplierText.text = "x" + GetMultiplier();
        }
    }
}