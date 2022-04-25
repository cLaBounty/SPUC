using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedCampfire : MonoBehaviour
{
    private const float HEALTH_INCREASE_PER_SECOND = 5f;
    private const float TIME_LIMIT = 20f;
    private const float RANGE = 5f;
    private const float RANGE_SQR = RANGE * RANGE;
    private const float FREQUENCY = 1f;

    private DeployedStatus status;

    private float totalTime = 0;
    private float coolDownTime = FREQUENCY;

    private Player player;

    void Start()
    {
        status = GetComponent<DeployedStatus>();
        player = GameObject.FindObjectOfType<Player>();
    }

    void Update()
    {
        if (!status.isActive) return;

        coolDownTime += Time.deltaTime;
        if (coolDownTime >= FREQUENCY) {
            coolDownTime = 0;

            float distance = (player.transform.position - transform.position).sqrMagnitude;
            if (distance <= RANGE) {
                player.GainHealth(HEALTH_INCREASE_PER_SECOND);
            }
        }

        totalTime += Time.deltaTime;
        if (totalTime >= TIME_LIMIT) {
            Destroy(transform.gameObject);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, RANGE);
    }
}
