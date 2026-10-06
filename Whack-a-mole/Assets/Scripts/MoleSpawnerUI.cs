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
    [SerializeField] private int moleCount = 1;
    [SerializeField] private float stayTime = 1.0f;
    [SerializeField] private float downAnimationTime = 0.5f;
    [SerializeField] private float spawnDelay = 0.7f;

    [Header("Mole Type Probability")]
    [Range(0f, 100f)]
    [SerializeField] private float normalChance = 70f;

    [Range(0f, 100f)]
    [SerializeField] private float specialChance = 20f;

    [Range(0f, 100f)]
    [SerializeField] private float badChance = 10f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 게임 종료 후에는 더 이상 두더지를 생성하지 않음
            if (GameManager.Instance != null &&
                GameManager.Instance.IsGameOver())
            {
                yield break;
            }

            Debug.Log("두더지 생성 시작");

            // 두더지 생성
            List<GameObject> spawnedMoles =
                SpawnMoles();

            // 두더지가 올라와 있는 시간
            yield return new WaitForSeconds(
                stayTime
            );

            // 게임 종료 확인
            if (GameManager.Instance != null &&
                GameManager.Instance.IsGameOver())
            {
                yield break;
            }

            Debug.Log("두더지 내려가기");

            // 두더지 내려가기
            for (int i = 0; i < spawnedMoles.Count; i++)
            {
                if (spawnedMoles[i] == null)
                {
                    continue;
                }

                MoveUI moveUI =
                    spawnedMoles[i].GetComponent<MoveUI>();

                if (moveUI != null)
                {
                    moveUI.HideMole();
                }
            }

            // 내려가는 애니메이션 대기
            yield return new WaitForSeconds(
                downAnimationTime
            );

            // 남아있는 두더지 삭제
            for (int i = 0; i < spawnedMoles.Count; i++)
            {
                if (spawnedMoles[i] != null)
                {
                    Destroy(spawnedMoles[i]);
                }
            }

            Debug.Log("두더지 삭제 완료");

            // 다음 두더지 생성까지 대기
            yield return new WaitForSeconds(
                spawnDelay
            );
        }
    }

    private List<GameObject> SpawnMoles()
    {
        List<GameObject> spawnedMoles =
            new List<GameObject>();

        if (holes == null || holes.Length == 0)
        {
            Debug.LogError(
                "MoleSpawnerUI: Hole이 연결되지 않았습니다."
            );

            return spawnedMoles;
        }

        // 사용할 수 있는 구멍 목록
        List<RectTransform> availableHoles =
            new List<RectTransform>();

        for (int i = 0; i < holes.Length; i++)
        {
            if (holes[i] != null)
            {
                availableHoles.Add(holes[i]);
            }
        }

        // 생성할 두더지 수
        int count = Mathf.Min(
            moleCount,
            availableHoles.Count
        );

        for (int i = 0; i < count; i++)
        {
            // 랜덤 구멍 선택
            int randomIndex =
                Random.Range(
                    0,
                    availableHoles.Count
                );

            RectTransform selectedHole =
                availableHoles[randomIndex];

            // 같은 구멍 중복 방지
            availableHoles.RemoveAt(
                randomIndex
            );

            // 두더지 종류 랜덤 선택
            GameObject selectedPrefab =
                GetRandomMolePrefab();

            if (selectedPrefab == null)
            {
                Debug.LogError(
                    "MoleSpawnerUI: 두더지 프리팹이 연결되지 않았습니다."
                );

                continue;
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

            // MoveUI 가져오기
            MoveUI moveUI =
                mole.GetComponent<MoveUI>();

            if (moveUI != null)
            {
                moveUI.ShowMole();
            }
            else
            {
                Debug.LogError(
                    mole.name +
                    "에 MoveUI가 없습니다."
                );
            }

            spawnedMoles.Add(mole);
        }

        return spawnedMoles;
    }

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
}