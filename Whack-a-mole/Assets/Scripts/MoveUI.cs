using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Image")]
    [SerializeField] private Image moleImage;

    [Header("Animation")]
    [SerializeField] private string upAnimationName = "Mole_Up";
    [SerializeField] private string downAnimationName = "Mole_Down";

    [Header("Hit")]
    [SerializeField] private Sprite hitSprite;

    [Tooltip("클릭된 스프라이트가 보여지는 시간")]
    [SerializeField] private float hitDisplayTime = 1.0f;

    [Header("Score")]
    [SerializeField] private int scoreValue = 1;

    [Header("Mole Type")]
    [SerializeField] private bool isBadMole = false;

    [Header("Bad Mole")]
    [SerializeField] private int badMoleScore = -2;

    private bool isActive;
    private bool isHit;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (moleImage == null)
        {
            moleImage = GetComponent<Image>();
        }
    }

    // 두더지 등장
    public void ShowMole()
    {
        isActive = true;
        isHit = false;

        if (moleImage != null)
        {
            moleImage.enabled = true;
        }

        if (animator != null)
        {
            animator.enabled = true;

            animator.Play(
                upAnimationName,
                0,
                0f
            );
        }
    }

    // 두더지가 내려감
    public void HideMole()
    {
        // 이미 클릭한 두더지는 무시
        if (isHit)
        {
            return;
        }

        isActive = false;

        // 일반 / 특수 두더지를 놓치면 콤보 초기화
        // 배드 두더지를 놓치는 것은 괜찮음
        if (!isBadMole)
        {
            if (ComboManager.Instance != null)
            {
                ComboManager.Instance.ResetCombo();
            }
        }

        // 내려가는 애니메이션
        if (animator != null)
        {
            animator.Play(
                downAnimationName,
                0,
                0f
            );
        }
    }

    // 두더지 클릭
    public void OnPointerClick(
        PointerEventData eventData
    )
    {
        // 게임 종료 후 클릭 방지
        if (GameManager.Instance != null &&
            GameManager.Instance.IsGameOver())
        {
            return;
        }

        // 두더지가 올라와 있지 않으면 무시
        if (!isActive)
        {
            return;
        }

        // 이미 클릭했다면 중복 처리 방지
        if (isHit)
        {
            return;
        }

        isHit = true;
        isActive = false;

        // =========================
        // 배드 두더지
        // =========================
        if (isBadMole)
        {
            // 배드 두더지를 클릭하면 콤보 초기화
            if (ComboManager.Instance != null)
            {
                ComboManager.Instance.RegisterBadHit();
            }

            // 점수 -2
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(
                    badMoleScore
                );
            }
        }
        // =========================
        // 일반 / 특수 두더지
        // =========================
        else
        {
            // 콤보 +1
            if (ComboManager.Instance != null)
            {
                ComboManager.Instance.RegisterNormalHit();
            }

            // 기본 배율
            int multiplier = 1;

            // 현재 콤보 배율 가져오기
            if (ComboManager.Instance != null)
            {
                multiplier =
                    ComboManager.Instance.GetMultiplier();
            }

            // 점수 × 배율
            int finalScore =
                scoreValue * multiplier;

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(
                    finalScore
                );
            }
        }

        // 클릭 후 처리
        StartCoroutine(HitRoutine());
    }

    // 클릭된 두더지 처리
    private IEnumerator HitRoutine()
    {
        // 애니메이션 정지
        if (animator != null)
        {
            animator.enabled = false;
        }

        // 클릭된 스프라이트로 변경
        if (hitSprite != null &&
            moleImage != null)
        {
            moleImage.sprite = hitSprite;
            moleImage.enabled = true;
        }

        // 클릭된 모습 유지
        yield return new WaitForSeconds(
            hitDisplayTime
        );

        // 두더지 삭제
        Destroy(gameObject);
    }

    // 현재 두더지가 활성 상태인지 확인
    public bool IsActive()
    {
        return isActive;
    }

    // 현재 두더지가 클릭됐는지 확인
    public bool IsHit()
    {
        return isHit;
    }

    // 배드 두더지인지 확인
    public bool IsBadMole()
    {
        return isBadMole;
    }
}