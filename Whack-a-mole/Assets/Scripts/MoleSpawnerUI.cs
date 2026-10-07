using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawnerUI : MonoBehaviour
{
    [Header("Holes")]
    [SerializeField] private RectTransform[] holes;

    [Header("Mole Prefabs")]
    [SerializeField] private GameObject normalMolePrefab;
    [SerializeField] private GameObject specialMolePrefab;
    [SerializeField] private GameObject badMolePrefab;

    [Header("Spawn Settings")]
    [Tooltip("새로운 두더지가 생성되는 간격")]
    [SerializeField] private float spawnInterval = 0.7f;

    [Tooltip("두더지가 올라와 있는 시간")]
    [SerializeField] private float stayTime = 1.0f;

    [Tooltip("두더지가 내려간 후 홀을 다시 사용할 수 있을 때까지의 시간")]
    [SerializeField] private float holeCooldown = 0.5f;

    [Tooltip("두더지가 내려가는 애니메이션 시간")]
    [SerializeField] private float downAnimationTime = 0.3f;

    [Header("Mole Type Probability")]
    [Range(0f, 100f)]
    [SerializeField] private float normalChance = 70f;

    [Range(0f, 100f)]
    [SerializeField] private float specialChance = 20f;

    [Range(0f, 100f)]
    [SerializeField] private float badChance = 10f;

    // 현재 두더지가 존재하는 홀
    private HashSet<RectTransform> occupiedHoles =
        new HashSet<RectTransform>();

    // 현재 쿨다운 중인 홀
    private Dictionary<RectTransform, float> holeCooldownTimers =
        new Dictionary<RectTransform, float>();

    private void Start()
    {
        RemoveNullHoles();

        StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        UpdateHoleCooldowns();
    }

    // =========================
    // 두더지 생성 루틴
    // =========================
    private IEnumerator SpawnRoutine()
    {
        // 게임 시작 직후 한 마리 생성
        SpawnMole();

        while (true)
        {
            // 게임 종료
            if (GameManager.Instance != null &&
                GameManager.Instance.IsGameOver())
            {
                yield break;
            }

            // 다음 생성까지 대기
            yield return new WaitForSeconds(
                spawnInterval
            );

            // 게임 종료
            if (GameManager.Instance != null &&
                GameManager.Instance.IsGameOver())
            {
                yield break;
            }

            // 두더지 생성
            SpawnMole();
        }
    }

    // =========================
    // 두더지 한 마리 생성
    // =========================
    private void SpawnMole()
    {
        List<RectTransform> availableHoles =
            GetAvailableHoles();

        // 사용할 수 있는 홀이 없으면 생성하지 않음
        if (availableHoles.Count == 0)
        {
            return;
        }

        // 랜덤 홀 선택
        int randomIndex =
            Random.Range(
                0,
                availableHoles.Count
            );

        RectTransform selectedHole =
            availableHoles[randomIndex];

        // 두더지 종류 선택
        GameObject selectedPrefab =
            GetRandomMolePrefab();

        if (selectedPrefab == null)
        {
            Debug.LogError(
                "MoleSpawnerUI: 두더지 프리팹이 연결되지 않았습니다."
            );

            return;
        }

        // 두더지 생성
        GameObject mole =
            Instantiate(
                selectedPrefab,
                selectedHole
            );

        // UI 위치 초기화
        RectTransform moleRect =
            mole.GetComponent<RectTransform>();

        if (moleRect != null)
        {
            moleRect.anchoredPosition =
                Vector2.zero;

            moleRect.localRotation =
                Quaternion.identity;

            moleRect.localScale =
                Vector3.one;
        }

        // 해당 홀 사용 중
        occupiedHoles.Add(
            selectedHole
        );

        // MoveUI 가져오기
        MoveUI moveUI =
            mole.GetComponent<MoveUI>();

        if (moveUI != null)
        {
            // Spawner와 홀 정보 연결
            moveUI.Initialize(
                this,
                selectedHole
            );

            moveUI.ShowMole();
        }
        else
        {
            Debug.LogError(
                mole.name +
                "에 MoveUI가 없습니다."
            );
        }

        // 두더지 자동 제거 처리
        StartCoroutine(
            MoleLifetime(
                mole,
                selectedHole
            )
        );
    }

    // =========================
    // 두더지 생존 시간
    // =========================
    private IEnumerator MoleLifetime(
        GameObject mole,
        RectTransform hole
    )
    {
        // 두더지가 올라와 있는 시간
        yield return new WaitForSeconds(
            stayTime
        );

        // 이미 클릭되어 삭제된 경우
        if (mole == null)
        {
            yield break;
        }

        // 게임 종료
        if (GameManager.Instance != null &&
            GameManager.Instance.IsGameOver())
        {
            Destroy(mole);

            ReleaseHole(hole);

            yield break;
        }

        // 두더지 내려가기
        MoveUI moveUI =
            mole.GetComponent<MoveUI>();

        if (moveUI != null)
        {
            moveUI.HideMole();
        }

        // 내려가는 애니메이션 대기
        yield return new WaitForSeconds(
            downAnimationTime
        );

        // 두더지 삭제
        if (mole != null)
        {
            Destroy(mole);
        }

        // 홀 사용 해제
        ReleaseHole(hole);
    }

    // =========================
    // 클릭해서 두더지가 삭제될 때 호출
    // =========================
    public void OnMoleDestroyed(
        RectTransform hole
    )
    {
        if (hole == null)
        {
            return;
        }

        // 사용 중인 홀 해제
        occupiedHoles.Remove(hole);

        // 쿨다운 시작
        StartHoleCooldown(hole);
    }

    // =========================
    // 사용 가능한 홀 가져오기
    // =========================
    private List<RectTransform> GetAvailableHoles()
    {
        List<RectTransform> availableHoles =
            new List<RectTransform>();

        for (int i = 0; i < holes.Length; i++)
        {
            RectTransform hole = holes[i];

            if (hole == null)
            {
                continue;
            }

            // 이미 두더지가 있는 홀
            if (occupiedHoles.Contains(hole))
            {
                continue;
            }

            // 쿨다운 중인 홀
            if (holeCooldownTimers.ContainsKey(hole))
            {
                continue;
            }

            availableHoles.Add(hole);
        }

        return availableHoles;
    }

    // =========================
    // 홀 사용 해제
    // =========================
    private void ReleaseHole(
        RectTransform hole
    )
    {
        if (hole == null)
        {
            return;
        }

        occupiedHoles.Remove(hole);

        StartHoleCooldown(hole);
    }

    // =========================
    // 홀 쿨다운 시작
    // =========================
    private void StartHoleCooldown(
        RectTransform hole
    )
    {
        if (hole == null)
        {
            return;
        }

        occupiedHoles.Remove(hole);

        if (holeCooldown <= 0f)
        {
            holeCooldownTimers.Remove(hole);
            return;
        }

        holeCooldownTimers[hole] =
            holeCooldown;
    }

    // =========================
    // 쿨다운 처리
    // =========================
    private void UpdateHoleCooldowns()
    {
        if (holeCooldownTimers.Count == 0)
        {
            return;
        }

        List<RectTransform> cooldownHoles =
            new List<RectTransform>(
                holeCooldownTimers.Keys
            );

        for (int i = 0;
             i < cooldownHoles.Count;
             i++)
        {
            RectTransform hole =
                cooldownHoles[i];

            if (!holeCooldownTimers.ContainsKey(hole))
            {
                continue;
            }

            holeCooldownTimers[hole] -=
                Time.deltaTime;

            if (holeCooldownTimers[hole] <= 0f)
            {
                holeCooldownTimers.Remove(hole);
            }
        }
    }

    // =========================
    // 두더지 종류 랜덤 선택
    // =========================
    private GameObject GetRandomMolePrefab()
    {
        float totalChance =
            normalChance +
            specialChance +
            badChance;

        if (totalChance <= 0f)
        {
            Debug.LogError(
                "MoleSpawnerUI: 두더지 확률의 합이 0입니다."
            );

            return null;
        }

        float randomValue =
            Random.Range(
                0f,
                totalChance
            );

        // 일반 두더지
        if (randomValue < normalChance)
        {
            return normalMolePrefab;
        }

        randomValue -= normalChance;

        // 특수 두더지
        if (randomValue < specialChance)
        {
            return specialMolePrefab;
        }

        // 배드 두더지
        return badMolePrefab;
    }

    // =========================
    // Null 홀 제거
    // =========================
    private void RemoveNullHoles()
    {
        if (holes == null)
        {
            holes =
                new RectTransform[0];

            return;
        }

        List<RectTransform> validHoles =
            new List<RectTransform>();

        for (int i = 0;
             i < holes.Length;
             i++)
        {
            if (holes[i] != null)
            {
                validHoles.Add(
                    holes[i]
                );
            }
        }

        holes =
            validHoles.ToArray();
    }
}