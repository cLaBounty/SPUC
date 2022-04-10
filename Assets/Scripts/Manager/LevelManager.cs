using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float initialPrepTime = 90f;
    public float timeBetweenWaves = 45f;
    public int numberOfWaves = 10;
    [SerializeField] int enemiesPerWave = 20;
    [SerializeField] int enemyMax = 40;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] string winScreen = "Victory";
    [SerializeField] string playerLossScreen = "PlayerDied";
    [SerializeField] string drillLossScreen = "OilDrillDied";

    GridController grid = null;
    Vector2 centerPos = Vector2.zero;

    //enemy variables
    OilDrill target;
    GridController flowField;
    PlayerMovement player;
    Player playerStats;
    
    public int waveCount = 0;
    public int enemyCount = 0;

    // Start is called before the first frame update
    void Start(){
        grid = GameObject.FindObjectOfType<GridController>();
        centerPos = (Vector2) grid.gridSize * grid.cellRadius;

        target          = GameObject.FindObjectOfType<OilDrill>();
        flowField       = GameObject.FindObjectOfType<GridController>();
        player          = GameObject.FindObjectOfType<PlayerMovement>();
        playerStats     = GameObject.FindObjectOfType<Player>();

        StartCoroutine(spawnEnemyTimer(initialPrepTime));
    }

    void Update(){
        if (enemyCount == 0 && waveCount >= numberOfWaves){
            //win
            playerStats.CleanUp(); // ToDo: only if last level
            SceneManager.LoadScene(winScreen);
        }
        else if (playerStats.currentHealth <= 0){
            //lose
            playerStats.CleanUp();
            SceneManager.LoadScene(playerLossScreen);
        }
        else if (target.currentHealth <= 0){
            //lose
            playerStats.CleanUp();
            SceneManager.LoadScene(drillLossScreen);
        }
    }

    IEnumerator spawnEnemyTimer(float time){
        yield return new WaitForSeconds(time);

        waveCount = waveCount + 1;
        SpawnEnemies();

        if (waveCount < numberOfWaves) StartCoroutine(spawnEnemyTimer(timeBetweenWaves));
    }

    void SpawnEnemies(){
        if (enemyCount > enemyMax) return;

        for (int i = 0; i < enemiesPerWave; ++i){
            var inst = Instantiate(enemyPrefab);
            inst.transform.parent = null;

            //adjust values
            //inst.target = target;
            Enemy enemy         = inst.GetComponent<Enemy>();
            enemy.target        = target;
            enemy.flowField     = flowField;
            enemy.playerStats   = playerStats;
            enemy.player        = player;
            enemy.levelManager  = this;

            enemyCount++;

            float randomDirection = Random.Range(0, 2 * Mathf.PI);
            inst.transform.position =   new Vector3(Mathf.Cos(randomDirection) * centerPos.x, 0, Mathf.Sin(randomDirection) * centerPos.y) +
                                        new Vector3(centerPos.x, 5, centerPos.y);

        }
    }

    public void EnemyKilled(){
        enemyCount--;
    }
}
