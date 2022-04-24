using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedEnemyAttractor : MonoBehaviour
{
    private const float TIME_LIMIT = 15f;
    private const float RANGE = 12f;
    private const float RANGE_SQR = RANGE * RANGE;
    private const float FREQUENCY = 1f;

    public bool isActive = false;
    private float totalTime = 0;
    private float coolDownTime = FREQUENCY;
    

    private GameObject oilDrill;

    private void Start() {
        oilDrill = GameObject.FindObjectOfType<OilDrill>().transform.gameObject;
    }

    private void Update() {
        if (!isActive) { return; }
        
        coolDownTime += Time.deltaTime;
        if (coolDownTime >= FREQUENCY) {
            coolDownTime = 0;
            AttractEnemies();
        }

        totalTime += Time.deltaTime;
        if (totalTime >= TIME_LIMIT) {
            Destroy(transform.gameObject);
        }
    }

    private void AttractEnemies() {
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in allEnemies) {
            Vector3 enemyPositionAtSameHeight = enemy.transform.position;
            enemyPositionAtSameHeight.y = transform.position.y;
            
            float distance = (enemyPositionAtSameHeight - transform.position).sqrMagnitude;
            if (distance <= RANGE_SQR) {
                if (enemy.state != Enemy.STATE.AGRO_DISTRACTION) {
                    enemy.target = transform.gameObject;
                    enemy.state = Enemy.STATE.AGRO_DISTRACTION;
                }
            }
        }
    }

    private void OnDestroy() {
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in allEnemies) {
            if (enemy.target == transform.gameObject) {
                enemy.target = oilDrill;
                enemy.state = Enemy.STATE.AGRO_OIL;
                enemy.isDistracted = false;
            }
        }
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, RANGE);
    }
}
