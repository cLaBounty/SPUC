using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAggravatorAttractor : MonoBehaviour
{
    [SerializeField] float range = 15f;
    [SerializeField] float frequency = 1f;
    [SerializeField] int enemyLimit = 3;
    [SerializeField] bool excludeFlyer = false;
    [SerializeField] bool excludeCrawler = false;
    [SerializeField] LayerMask enemyMask;

    private GameObject oilDrill;
    private List<Enemy> enemyList;
    private DeployedStatus deployedStatus;
    
    private float rangeSqr;
    private float totalTime = 0;
    private float coolDownTime;

    private void Start() {
        oilDrill = GameObject.FindObjectOfType<OilDrill>().transform.gameObject;
        rangeSqr = range * range;
        coolDownTime = frequency;
        enemyList = new List<Enemy>();
        deployedStatus = GetComponent<DeployedStatus>();
    }

    private void Update() {
        if (!deployedStatus.isActive) return;
        
        coolDownTime += Time.deltaTime;
        if (coolDownTime >= frequency) {
            coolDownTime = 0;
            AttractEnemies();
        }

        for (int i = enemyList.Count - 1; i > -1; --i)
            if (enemyList[i] == null) enemyList.RemoveAt(i);
        
        totalTime += Time.deltaTime;
    }

    private void AttractEnemies() {
        int getNum = enemyLimit - enemyList.Count;

        if (getNum > 0){
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, enemyMask);
            getNum = Mathf.Min(colliders.Length, getNum);

            for (int i = 0; i < getNum; ++i){
                Enemy currentEnemy = colliders[i].gameObject.GetComponent<Enemy>();
                if (currentEnemy == null) continue;
                if (currentEnemy.state == Enemy.STATE.AGRO_DISTRACTION || currentEnemy.state == Enemy.STATE.DEAD) continue;

                if (currentEnemy.isFlying && !excludeFlyer) enemyList.Add(currentEnemy);
                else if (!currentEnemy.isFlying && !excludeCrawler) enemyList.Add(currentEnemy);
            }
        }

        foreach(Enemy enemy in enemyList){
            enemy.target = transform.gameObject;
            enemy.state = Enemy.STATE.AGRO_DISTRACTION;
        }
    }

    private void OnDestroy() {
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in allEnemies) {
            if (enemy != null)
            if (enemy.target == transform.gameObject) {
                enemy.target = oilDrill;
                enemy.state = Enemy.STATE.AGRO_OIL;
                enemy.CheckDeadState();
                enemy.isDistracted = false;
            }
        }
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
