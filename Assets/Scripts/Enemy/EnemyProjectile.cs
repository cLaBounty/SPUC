using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float projectileSpeed;
    public GameObject target;
    public Vector3 moveDirection;
    public float steeringSpeed = 1f;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null){
            Vector3 directionToPlayer = target.transform.position - transform.position;
            moveDirection = Vector3.RotateTowards(moveDirection, directionToPlayer, steeringSpeed * Time.deltaTime, 1f);
            moveDirection.Normalize();
        }

        rb.velocity = moveDirection * projectileSpeed;
    }
}
