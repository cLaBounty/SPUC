using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody))] 
public class CrawlerEnemy : Enemy
{
    [SerializeField]float agroDistance = 10f;
    [SerializeField]float attackDistance = 1f;
    [SerializeField]float damping = 0.98f;

    NavMeshAgent navMeshAgent;
    Rigidbody rb;
    GridController flowField = null;
    PlayerMovement player = null;
    Player playerStats = null;
    Vector3 acculmulatedSpeed = Vector3.zero;

    float agroRangeSqr = 0;
    float attackRangeSqr = 0;
    float coolDown = 0;
    float coolDownMax = 2f;

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
        
        //set navmeshagent
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    // Update is called once per frame
    private void Update() {
        if (player != null){
            float range = (player.transform.position - transform.position).sqrMagnitude;

            // ToDo: attack oil drill
            
            if (range < attackRangeSqr) AttackPlayer();
            else if (range < agroRangeSqr) MoveTowardsPlayer(new Vector2(player.transform.position.x, player.transform.position.z));
            else MoveTowardsTarget();
        }
        else MoveTowardsTarget();

        if (coolDown >= 0)
            coolDown -= Time.deltaTime;
    }

    void AttackPlayer(){
        if (coolDown < 0) {
            playerStats.TakeDamage(4);
            coolDown = coolDownMax;
        }
    }

    void AttackOilDrill(){
        if (coolDown < 0) {
            OilDrill oilDrill = target.GetComponent<OilDrill>();
            oilDrill.TakeDamage(4);
            coolDown = coolDownMax;
        }
    }

    void MoveTowardsTarget(){
        if (flowField.initialized){
            //Debug.Log("Target");
            Cell occupideCell = flowField.curFlowField.GetCellFromWorldPos(transform.position);
            Vector3 moveDirection;

            if (occupideCell.cost == 255){
                navMeshAgent.destination = target.transform.position;
                return;
            }
            else{
                moveDirection = new Vector3(occupideCell.bestDirection.x, 0, occupideCell.bestDirection.y);
            }

            acculmulatedSpeed *= damping;
            acculmulatedSpeed += moveDirection * moveSpeed * Time.fixedDeltaTime;
            acculmulatedSpeed = Vector3.ClampMagnitude(acculmulatedSpeed, maxMoveSpeed);
            rb.velocity = new Vector3(acculmulatedSpeed.x, rb.velocity.y, acculmulatedSpeed.z);
        }
        else{
            Debug.Log("Flow Field not Initialized");
        }
    }

    void MoveTowardsPlayer(Vector2 playerTarget){
        //Debug.Log("Player");
        //Vector2 direction = new Vector2(playerTarget.x - transform.position.x, playerTarget.y - transform.position.z);
        //acculmulatedSpeed = new Vector3(direction.x, 0, direction.y).normalized * maxMoveSpeed;
        //rb.velocity = new Vector3(direction.x, 0, direction.y).normalized * maxMoveSpeed * Time.fixedDeltaTime * 50;
        navMeshAgent.destination = player.transform.position;
    }
}
