using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedEnemyAttractor : MonoBehaviour
{
    private const float TIME_LIMIT = 15f;
    private const float RANGE = 100f;
    private const float FREQUENCY = 1f;

    public bool isActive = false;
    private float totalTime = 0;
    private float coolDownTime = FREQUENCY;

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
            float distance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distance <= RANGE) {
                if (enemy.target.GetComponent<DeployedEnemyAttractor>() == null) {
                    enemy.target = transform.gameObject;
                }
            }
        }
    }
}
