using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
public class CrawlerEnemy : Enemy
{
    Rigidbody rb;
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Target");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.z - transform.position.z);
        rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.y);
    }
}
