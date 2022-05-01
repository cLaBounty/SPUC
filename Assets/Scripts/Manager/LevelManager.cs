using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EndCause {
    Win,
    PlayerLoss,
    OilDrillLoss
}

public class LevelManager : MonoBehaviour
{
    public static EndCause endCause;

    #region overhead

    [System.Serializable]
    public struct EnemyData{
        public GameObject enemyPrefab;
        [Range(1, 20)] public int spawnWeight;

        [HideInInspector]
        public int trueSpawnWeight; 
    }

    [Header("Core")]
    public float initialPrepTime = 90f;
    public float timeBetweenWaves = 45f;
    public int numberOfWaves = 10;
    [SerializeField] EnemyData[] enemyData;
    [SerializeField] int enemiesPerWave = 10;
    [SerializeField] int enemyMax = 60;

    [Header("Scene Managment")]
    [SerializeField] string endScene = "EndScene";

    [Header("Enemy Level Scale")]
    public int enemyLevel = 1;
    [Range(1, 2)][SerializeField] float waveIncreasePerLevel = 1.2f;
    [Range(1, 2)][SerializeField] float hpIncreasePerLevel = 1.1f;
    [Range(1, 2)][SerializeField] float defenseIncreasePerLevel = 1.05f;
    [Range(1, 2)][SerializeField] float attackIncreasePerLevel = 1.2f;

    float currentwaveIncrease = 1f;
    float currentHpIncrease = 1f;
    float defenseIncrease = 1f;
    float attackIncreaseIncrease = 1f;

    //management stuff
    Queue<GameObject> enemyQue;
    bool enqueuingNextWave = true;
    bool spawningNewEnemies = false;
    bool firstEnemyOfWave = true;
    int sumWeight = 0;

    GridController grid = null;
    Vector2 centerPos = Vector2.zero;

    //enemy variables
    OilDrill oilDrill;
    GridController flowField;
    PlayerMovement player;
    Player playerStats;
    
    [HideInInspector]
    public int waveCount = 0;
    [HideInInspector]
    public int enemyCount = 0;

    SupplyDropSpawner supplyDropSpawner;

    #endregion

    // Start is called before the first frame update
    void Start(){
        grid = GameObject.FindObjectOfType<GridController>();
        centerPos = (Vector2) grid.gridSize * grid.cellRadius;

        oilDrill          = GameObject.FindObjectOfType<OilDrill>();
        flowField         = GameObject.FindObjectOfType<GridController>();
        player            = GameObject.FindObjectOfType<PlayerMovement>();
        playerStats       = GameObject.FindObjectOfType<Player>();
        supplyDropSpawner = GameObject.FindObjectOfType<SupplyDropSpawner>();

        //create enemy que
        enemyQue = new Queue<GameObject>();

        //get max weight
        UpdateMaxWeight();

        StartCoroutine(spawnEnemyTimer(initialPrepTime));
    }

    void Update(){
        CheckGameStates();
        EnqueueEnemies();
        SpawnEnemies();
    }

    IEnumerator spawnEnemyTimer(float time){
        yield return new WaitForSeconds(time);
        if (spawningNewEnemies) StartCoroutine(spawnEnemyTimer(1f));
        else{
            spawningNewEnemies = true;
            firstEnemyOfWave = true;
            if (waveCount + 1 < numberOfWaves) StartCoroutine(spawnEnemyTimer(timeBetweenWaves));
        }
    }

    void SpawnEnemies(){
        if (!spawningNewEnemies || enemyCount > enemyMax) return;

        if (enemyQue.Count == 0){
            spawningNewEnemies = false;
            enqueuingNextWave = true;
            return;
        }

        GameObject enemyPrefab = enemyQue.Dequeue();

        float randomDirection = Random.Range(0, 2 * Mathf.PI);
        Vector3 newPos = new Vector3(Mathf.Cos(randomDirection) * centerPos.x, 0, Mathf.Sin(randomDirection) * centerPos.y) + new Vector3(centerPos.x, 5, centerPos.y);

        var inst = Instantiate(enemyPrefab, newPos, Quaternion.identity);
        inst.transform.parent = null;

        //adjust values
        Enemy enemy        = inst.GetComponent<Enemy>();
        enemy.target       = oilDrill.transform.gameObject;
        enemy.flowField    = flowField;
        enemy.playerStats  = playerStats;
        enemy.player       = player;
        enemy.levelManager = this;
        enemy.SetHealth(enemy.currentHealth * currentHpIncrease);
        enemy.attackPower  *= attackIncreaseIncrease;
        enemy.defense      *= defenseIncrease;

        enemyCount++;

        inst.transform.position = newPos;

        if (firstEnemyOfWave){
            waveCount++;
            Player.WavesCompleted += 1;
            enemyLevel++;

            currentwaveIncrease     *= waveIncreasePerLevel;
            currentHpIncrease       *= hpIncreasePerLevel;
            defenseIncrease         *= defenseIncreasePerLevel;
            attackIncreaseIncrease  *= attackIncreasePerLevel;

            firstEnemyOfWave = false;

            supplyDropSpawner.SpawnBeforeWave();
        }
    }

    void EnqueueEnemies(){
        if (!enqueuingNextWave) return;

        bool found = false;
        int maxLoop = Mathf.Min(enemyData.Length, waveCount + 1);
        int tempSumWeight = sumWeight;

        for (int i = enemyData.Length - 1; i > maxLoop; --i){
            tempSumWeight -= enemyData[i].spawnWeight;
        }

        int ranEnemyRange = Random.Range(0, tempSumWeight);

        for (int i = 0; i < maxLoop; ++i){
            if (ranEnemyRange < enemyData[i].trueSpawnWeight){
                enemyQue.Enqueue(enemyData[i].enemyPrefab);
                found = true;
                break;
            }
        }

        if (!found) enemyQue.Enqueue(enemyData[0].enemyPrefab);
        if (enemyQue.Count >= enemiesPerWave * currentwaveIncrease) enqueuingNextWave = false;
    }

    void CheckGameStates(){
        if (enemyCount == 0 && waveCount >= numberOfWaves && !spawningNewEnemies) { // Win
            playerStats.CleanUp(); // ToDo: only if last level
            endCause = EndCause.Win;
            SceneManager.LoadScene(endScene);
        }
        else if (playerStats.currentHealth <= 0) { // Lose via Player Health
            playerStats.CleanUp();
            endCause = EndCause.PlayerLoss;
            SceneManager.LoadScene(endScene);
        }
        else if (oilDrill.currentHealth <= 0) { // Lose via Oil Drill
            playerStats.CleanUp();
            endCause = EndCause.OilDrillLoss;
            SceneManager.LoadScene(endScene);
        }
    }

    void UpdateMaxWeight(){
        sumWeight = 0;
        for (int i = 0; i < enemyData.Length; ++i){
            sumWeight += enemyData[i].spawnWeight;
            enemyData[i].trueSpawnWeight = sumWeight;
        }
    }

    public void EnemyKilled(){
        enemyCount--;

        if (enemyCount <= 0) {
            supplyDropSpawner.SpawnAfterWave();
        }
    }
}
