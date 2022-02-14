using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float timer = 60f;
    [SerializeField] int amount = 20;
    [SerializeField] GameObject enemyPrefab;

    GridController grid = null;
    Vector2 centerPos = Vector2.zero;


    // Start is called before the first frame update
    void Start(){
        StartCoroutine(spawnEnemyTimer(timer));
        grid = GameObject.FindObjectOfType<GridController>();
        centerPos = (Vector2) grid.gridSize * grid.cellRadius;
    }

    IEnumerator spawnEnemyTimer(float time){
        yield return new WaitForSeconds(time);
        SpawnEnemies();
        StartCoroutine(spawnEnemyTimer(timer));
    }

    void SpawnEnemies(){
        if (GameObject.FindObjectsOfType<CrawlerEnemy>().Length > 10) return;

        for (int i = 0; i < amount; ++i){
            var inst = Instantiate(enemyPrefab);
            inst.transform.parent = null;

            float randomDirection = Random.Range(0, 2 * Mathf.PI);
            inst.transform.position = new Vector3(Mathf.Cos(randomDirection) + centerPos.x, 0, Mathf.Sin(randomDirection) + centerPos.y);
            inst.transform.position = new Vector3(inst.transform.position.x, 5f, inst.transform.position.y);
        }
    }
}
