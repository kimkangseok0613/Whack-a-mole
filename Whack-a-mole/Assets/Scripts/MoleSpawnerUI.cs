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
    [SerializeField] private float stayTime = 1f;
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
        while (!IsGameOver())
        {
            List<GameObject> spawnedMoles = SpawnMoles();

            yield return new WaitForSeconds(stayTime);

            if (IsGameOver())
            {
                yield break;
            }

            HideMoles(spawnedMoles);

            yield return new WaitForSeconds(downAnimationTime);

            DestroyMoles(spawnedMoles);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private List<GameObject> SpawnMoles()
    {
        List<GameObject> spawnedMoles = new List<GameObject>();

        if (holes == null || holes.Length == 0)
        {
            Debug.LogError(
                "MoleSpawnerUI: Hole이 연결되지 않았습니다."
            );

            return spawnedMoles;
        }

        List<RectTransform> availableHoles =
            GetAvailableHoles();

        int spawnCount =
            Mathf.Min(moleCount, availableHoles.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            RectTransform hole =
                GetRandomHole(availableHoles);

            GameObject prefab =
                GetRandomMolePrefab();

            if (hole == null || prefab == null)
            {
                continue;
            }

            GameObject mole =
                CreateMole(prefab, hole);

            if (mole != null)
            {
                spawnedMoles.Add(mole);
            }
        }

        return spawnedMoles;
    }

    private List<RectTransform> GetAvailableHoles()
    {
        List<RectTransform> availableHoles =
            new List<RectTransform>();

        foreach (RectTransform hole in holes)
        {
            if (hole != null)
            {
                availableHoles.Add(hole);
            }
        }

        return availableHoles;
    }

    private RectTransform GetRandomHole(
        List<RectTransform> availableHoles)
    {
        if (availableHoles.Count == 0)
        {
            return null;
        }

        int index =
            Random.Range(0, availableHoles.Count);

        RectTransform selectedHole =
            availableHoles[index];

        availableHoles.RemoveAt(index);

        return selectedHole;
    }

    private GameObject CreateMole(
        GameObject prefab,
        RectTransform hole)
    {
        GameObject mole =
            Instantiate(prefab, hole);

        RectTransform moleRect =
            mole.GetComponent<RectTransform>();

        if (moleRect != null)
        {
            moleRect.anchoredPosition = Vector2.zero;
            moleRect.localRotation = Quaternion.identity;
            moleRect.localScale = Vector3.one;
        }

        MoveUI moveUI =
            mole.GetComponent<MoveUI>();

        if (moveUI == null)
        {
            Debug.LogError(
                mole.name +
                "에 MoveUI가 없습니다."
            );

            Destroy(mole);
            return null;
        }

        moveUI.ShowMole();

        return mole;
    }

    private void HideMoles(
        List<GameObject> spawnedMoles)
    {
        foreach (GameObject mole in spawnedMoles)
        {
            if (mole == null)
            {
                continue;
            }

            MoveUI moveUI =
                mole.GetComponent<MoveUI>();

            if (moveUI != null)
            {
                moveUI.HideMole();
            }
        }
    }

    private void DestroyMoles(
        List<GameObject> spawnedMoles)
    {
        foreach (GameObject mole in spawnedMoles)
        {
            if (mole != null)
            {
                Destroy(mole);
            }
        }
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
            Random.Range(0f, totalChance);

        if (randomValue < normalChance)
        {
            return normalMolePrefab;
        }

        randomValue -= normalChance;

        if (randomValue < specialChance)
        {
            return specialMolePrefab;
        }

        return badMolePrefab;
    }

    private bool IsGameOver()
    {
        return GameManager.Instance != null &&
               GameManager.Instance.IsGameOver();
    }
}