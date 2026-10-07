using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance;

    [Header("Combo")]
    [SerializeField] private int combo = 0;

    [Tooltip("몇 콤보마다 배율이 증가하는지")]
    [SerializeField] private int comboStep = 10;

    [Tooltip("최대 배율")]
    [SerializeField] private int maxMultiplier = 5;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI multiplierText;

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

        UpdateComboUI();
    }

    // 일반 / 특수 두더지를 잡았을 때
    public void RegisterNormalHit()
    {
        AddCombo();
    }

    // 배드 두더지를 잡았을 때
    public void RegisterBadHit()
    {
        // 배드 두더지를 클릭하면 콤보 초기화
        ResetCombo();
    }

    // 콤보 +1
    public void AddCombo()
    {
        combo++;

        UpdateComboUI();
    }

    // 콤보 초기화
    public void ResetCombo()
    {
        combo = 0;

        UpdateComboUI();
    }

    // 현재 콤보 반환
    public int GetCombo()
    {
        return combo;
    }

    // 현재 배율 반환
    public int GetMultiplier()
    {
        // 0 ~ 9 콤보
        if (combo < comboStep)
        {
            return 1;
        }

        // 10 ~ 19 = 2배
        // 20 ~ 29 = 3배
        // 30 ~ 39 = 4배
        // 40 이상 = 5배
        int multiplier =
            (combo / comboStep) + 1;

        return Mathf.Min(
            multiplier,
            maxMultiplier
        );
    }

    // 콤보 / 배율 UI 갱신
    private void UpdateComboUI()
    {
        if (comboText != null)
        {
            comboText.text =
                "콤보 : " + combo;
        }

        if (multiplierText != null)
        {
            multiplierText.text =
                "x" + GetMultiplier();
        }
    }
}