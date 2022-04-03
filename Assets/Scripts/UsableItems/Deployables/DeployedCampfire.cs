using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedCampfire : MonoBehaviour
{
    private const float HEALTH_INCREASE_PER_SECOND = 5f;
    private const float TIME_LIMIT = 20f;
    private const float RANGE = 25f;

    public bool isActive = false;
    private float totalTime = 0;
    private float coolDownTime = 0;

    private Player player;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    void Update()
    {
        if (!isActive) { return; }

        coolDownTime += Time.deltaTime;
        if (coolDownTime >= 1f) {
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
}
