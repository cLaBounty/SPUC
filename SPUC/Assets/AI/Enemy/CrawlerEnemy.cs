using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
public class CrawlerEnemy : Enemy
{
    [SerializeField]float agroDistance = 10f;
    [SerializeField]float attackDistance = 1f;

    Rigidbody rb;
    GameObject target;
    GridController flowField = null;
    PlayerMovement player = null;
    Player playerStats = null;

    float agroRangeSqr = 0;
    float attackRangeSqr = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb              = GetComponent<Rigidbody>();
        target          = GameObject.FindGameObjectWithTag("target");
        flowField       = GameObject.FindObjectOfType<GridController>();
        player          = GameObject.FindObjectOfType<PlayerMovement>();
        playerStats     = GameObject.FindObjectOfType<Player>();
        agroRangeSqr    = agroDistance * agroDistance;
        attackRangeSqr  = attackDistance * attackDistance;
    }

    // Update is called once per frame
    void Update()
    {
        float range = (player.transform.position - transform.position).sqrMagnitude;

        if (player != null){
            if (range < attackRangeSqr) Attack();
            else if (range < agroRangeSqr) MoveTowardsTarget();
            else MoveTowardsTarget();
        }
        else MoveTowardsTarget();
    }

    void Attack(){
        //attack player here
    }

    void MoveTowardsTarget(){
        if (flowField.initialized){
            Cell occupideCell = flowField.curFlowField.GetCellFromWorldPos(transform.position);
            //Debug.Log("Cell: " + occupideCell.gridIndex);
            Vector3 moveDirection = new Vector3(occupideCell.bestDirection.x, 0, occupideCell.bestDirection.y);
            //Debug.Log("Dir: " + moveDirection);
            rb.velocity = moveDirection * moveSpeed;
        }
        else{
            Debug.Log("Flow Field not Initialized");
        }
    }

    void MoveTowardsPlayer(){
        Vector2 direction = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.z - transform.position.z);
        rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.y) * moveSpeed;
    }
}
