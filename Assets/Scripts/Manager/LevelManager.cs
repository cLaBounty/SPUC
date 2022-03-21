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

    //enemy variables
    OilDrill target;
    GridController flowField;
    PlayerMovement player;
    Player playerStats;


    // Start is called before the first frame update
    void Start(){
        grid = GameObject.FindObjectOfType<GridController>();
        centerPos = (Vector2) grid.gridSize * grid.cellRadius;

        target          = GameObject.FindObjectOfType<OilDrill>();
        flowField       = GameObject.FindObjectOfType<GridController>();
        player          = GameObject.FindObjectOfType<PlayerMovement>();
        playerStats     = GameObject.FindObjectOfType<Player>();

        StartCoroutine(spawnEnemyTimer(timer));
    }

    IEnumerator spawnEnemyTimer(float time){
        SpawnEnemies();
        yield return new WaitForSeconds(time);
        StartCoroutine(spawnEnemyTimer(timer));
    }

    void SpawnEnemies(){
        if (GameObject.FindObjectsOfType<CrawlerEnemy>().Length > 10) return;

        for (int i = 0; i < amount; ++i){
            var inst = Instantiate(enemyPrefab);
            inst.transform.parent = null;

        
            //adjust values
            //inst.target = target;
            Enemy enemy         = inst.GetComponent<Enemy>();
            enemy.target        = target;
            enemy.flowField     = flowField;
            enemy.playerStats   = playerStats;
            enemy.player        = player;

            float randomDirection = Random.Range(0, 2 * Mathf.PI);
            inst.transform.position =   new Vector3(Mathf.Cos(randomDirection) * centerPos.x, 0, Mathf.Sin(randomDirection) * centerPos.y) +
                                        new Vector3(centerPos.x, 5, centerPos.y);

        }
    }
}
