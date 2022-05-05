using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float projectileSpeed;
    public GameObject target;
    public Vector3 moveDirection;
    public float steeringSpeed = 1f;
    public float damage = 10f;

    Rigidbody rb;
    Vector3 directionToPlayer;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 playerPosition = target.transform.position;
        playerPosition.y += 0.75f; // Target player waist, not feet
        directionToPlayer = playerPosition - transform.position;

        if (target != null){
            moveDirection = Vector3.RotateTowards(moveDirection, directionToPlayer, steeringSpeed * Time.deltaTime, 1f);
            moveDirection.Normalize();
        }

        rb.velocity = moveDirection * projectileSpeed;
    }
}
