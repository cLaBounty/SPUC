using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedEnemyAttractor : MonoBehaviour
{
    [SerializeField] private float timeLimit = 30f;
    [SerializeField] private float range = 12f;
    [SerializeField] private float frequency = 1f;

    private DeployedStatus status;
    private GameObject oilDrill;

    private float rangeSqr;
    private float coolDownTime;
    private float totalTime = 0;
    
    private void Start() {
        status = GetComponent<DeployedStatus>();
        oilDrill = GameObject.FindObjectOfType<OilDrill>().transform.gameObject;
        rangeSqr = range * range;
        coolDownTime = frequency;
    }

    private void Update() {
        if (!status.isActive) return;
        
        coolDownTime += Time.deltaTime;
        if (coolDownTime >= frequency) {
            coolDownTime = 0;
            AttractEnemies();
        }

        totalTime += Time.deltaTime;
        if (totalTime >= timeLimit) {
            Destroy(transform.gameObject);
        }
    }

    private void AttractEnemies() {
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in allEnemies) {
            Vector3 enemyPositionAtSameHeight = enemy.transform.position;
            enemyPositionAtSameHeight.y = transform.position.y;
            
            float distance = (enemyPositionAtSameHeight - transform.position).sqrMagnitude;
            if (distance <= rangeSqr) {
                if (enemy.state != Enemy.STATE.AGRO_DISTRACTION && enemy.state != Enemy.STATE.DEAD) {
                    enemy.target = transform.gameObject;
                    enemy.state = Enemy.STATE.AGRO_DISTRACTION;
                }
            }
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
