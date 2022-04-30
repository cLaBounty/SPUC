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
            Enable();
        }

        totalTime += Time.deltaTime;
        if (totalTime >= timeLimit) {
            Destroy(transform.gameObject);
        }
    }

    private void Enable() {
        // Player
        float playerDistance = (player.transform.position - transform.position).sqrMagnitude;
        if (playerDistance <= rangeSqr) {
            player.GainHealth(healthIncreasePerSecond);
        }

        // Robots
        List<Enemy> livingRobots = player.GetLivingRobots();
        foreach(Enemy robot in livingRobots) {
            float distance = (robot.transform.position - transform.position).sqrMagnitude;
            if (distance <= rangeSqr) {
                robot.GainHealth(healthIncreasePerSecond);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
