using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region overhead
    //stucts
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
    [SerializeField] string winScreen = "Victory";
    [SerializeField] string playerLossScreen = "PlayerDied";
    [SerializeField] string drillLossScreen = "OilDrillDied";


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

    #endregion

    // Start is called before the first frame update
    void Start(){
        grid = GameObject.FindObjectOfType<GridController>();
        centerPos = (Vector2) grid.gridSize * grid.cellRadius;

        oilDrill        = GameObject.FindObjectOfType<OilDrill>();
        flowField       = GameObject.FindObjectOfType<GridController>();
        player          = GameObject.FindObjectOfType<PlayerMovement>();
        playerStats     = GameObject.FindObjectOfType<Player>();

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

        var inst = Instantiate(enemyPrefab);
        inst.transform.parent = null;

        //adjust values
        Enemy enemy         = inst.GetComponent<Enemy>();
        enemy.target        = oilDrill.transform.gameObject;
        enemy.flowField     = flowField;
        enemy.playerStats   = playerStats;
        enemy.player        = player;
        enemy.levelManager  = this;
        //enemy.SetHealth(enemy.currentHealth * currentHpIncrease);
        //enemy.attackPower   *= attackIncreaseIncrease;
        //enemy.defense       *= defenseIncrease;

        enemyCount++;

        float randomDirection = Random.Range(0, 2 * Mathf.PI);
        inst.transform.position =   new Vector3(Mathf.Cos(randomDirection) * centerPos.x, 0, Mathf.Sin(randomDirection) * centerPos.y) +
                                    new Vector3(centerPos.x, 5, centerPos.y);

        if (firstEnemyOfWave){
            waveCount++;
            enemyLevel++;

            currentwaveIncrease     *= waveIncreasePerLevel;
            currentHpIncrease       *= hpIncreasePerLevel;
            defenseIncrease         *= defenseIncreasePerLevel;
            attackIncreaseIncrease  *= attackIncreasePerLevel;

            firstEnemyOfWave = false;
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
        if (enemyCount == 0 && waveCount >= numberOfWaves && !spawningNewEnemies){
            //win
            playerStats.CleanUp(); // ToDo: only if last level
            SceneManager.LoadScene(winScreen);
        }
        else if (playerStats.currentHealth <= 0){
            //lose
            playerStats.CleanUp();
            SceneManager.LoadScene(playerLossScreen);
        }
        else if (oilDrill.currentHealth <= 0){
            //lose
            playerStats.CleanUp();
            SceneManager.LoadScene(drillLossScreen);
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
    }
}
