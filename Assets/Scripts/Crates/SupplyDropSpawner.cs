using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDropSpawner : MonoBehaviour
{
    [SerializeField] GameObject supplyDropPrefab;
    [SerializeField] float startHeight = 100f;

    [SerializeField] int spawnBeforeCount = 1;
    [Range(0,1)][SerializeField] float spawnBeforeRate = 0.5f;
    
    [SerializeField] int spawnAfterCount = 3;
    [Range(0,1)][SerializeField] float spawnAfterRate = 0.75f;

    private List<int> spawnedIndexes = new List<int>();

    public void SpawnBeforeWave() {
        for (int i = 0; i < spawnBeforeCount; i++) {
            if (Random.Range(0f, 1f) <= spawnBeforeRate) { Spawn(); }
        }
    }

    public void SpawnAfterWave() {
        for (int i = 0; i < spawnAfterCount; i++) {
            if (Random.Range(0f, 1f) <= spawnAfterRate) { Spawn(); }
        }
    }

    private void Spawn() {
        GameObject inst = Instantiate(supplyDropPrefab);
        inst.transform.parent = null;
        inst.transform.position = SelectRandomPosition();
        inst.GetComponent<Crate>().SetRarity((CrateRarity)Random.Range(0, 3));
    }

    private Vector3 SelectRandomPosition() {
        int randomIndex = Random.Range(0, transform.childCount);
        while (IsSpawned(randomIndex)) {
            randomIndex = (randomIndex + 1) % transform.childCount;
        }
        spawnedIndexes.Add(randomIndex);

        Vector3 groundPosition = transform.GetChild(randomIndex).position;
        return new Vector3(groundPosition.x, startHeight, groundPosition.z);
    }

    private bool IsSpawned(int index) {
        if (spawnedIndexes.Count == transform.childCount) { spawnedIndexes.Clear(); }
        foreach(int spawnedIndex in spawnedIndexes) {
            if (spawnedIndex == index) { return true; }
        }
        return false;
    }
}
