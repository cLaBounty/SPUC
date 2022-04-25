using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedCampfire : MonoBehaviour
{
    [SerializeField] private float healthIncreasePerSecond = 5f;
    [SerializeField] private float timeLimit = 20f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float frequency = 1f;

    private DeployedStatus status;
    private Player player;

    private float rangeSqr;
    private float coolDownTime;
    private float totalTime = 0;

    void Start()
    {
        status = GetComponent<DeployedStatus>();
        player = GameObject.FindObjectOfType<Player>();
        rangeSqr = range * range;
        coolDownTime = frequency;
    }

    void Update()
    {
        if (!status.isActive) return;

        coolDownTime += Time.deltaTime;
        if (coolDownTime >= frequency) {
            coolDownTime = 0;

            float distance = (player.transform.position - transform.position).sqrMagnitude;
            if (distance <= rangeSqr) {
                player.GainHealth(healthIncreasePerSecond);
            }
        }

        totalTime += Time.deltaTime;
        if (totalTime >= timeLimit) {
            Destroy(transform.gameObject);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
