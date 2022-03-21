using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody))] 
public class FlyingEnemy : Enemy
{
    [SerializeField]float agroDistance = 0f;
    [SerializeField]float attackDistance = 1f;
    [SerializeField]float damping = 0.98f;
    [SerializeField]float maxFlyHeight = 10f;
    [SerializeField]float minFlyHeight = 3f;
    [SerializeField]float startUpwardDist = 20f;
    [SerializeField]float useNavMeshDist = 3f;

    NavMeshAgent navMeshAgent;
    Rigidbody rb;
    //GridController flowField = null;
    //PlayerMovement player = null;
    //Player playerStats = null;
    Vector3 acculmulatedSpeed = Vector3.zero;

    float agroRangeSqr = 0;
    float attackRangeSqr = 0;
    float coolDown = 0;
    float coolDownMax = 2f;
    
    int impassableMask;

    // Start is called before the first frame update
    void Start()
    {
        rb              = GetComponent<Rigidbody>();
        agroRangeSqr    = agroDistance * agroDistance;
        attackRangeSqr  = attackDistance * attackDistance;
        
        //set navmeshagent
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        
        //impassable mask
        impassableMask = LayerMask.GetMask("Impassible Terrain");
    }

    // Update is called once per frame
    private void Update() {
        if (player != null){
            float range = (player.transform.position - transform.position).sqrMagnitude;
            
            //Debug.Log("agroRangeSqr " + agroRangeSqr);
            //Debug.Log("attackRangeSqr " + attackRangeSqr);
            //Debug.Log("range " + range);

            if (range < attackRangeSqr) Attack();
            else if (range < agroRangeSqr) MoveTowardsPlayer();
            else MoveTowardsTarget();
        }
        else MoveTowardsTarget();

        if (coolDown >= 0)
            coolDown -= Time.deltaTime;

        //Debug.Log("Enemy Speed" + acculmulatedSpeed);
    }

    void Attack(){
        Vector3 dir =  (player.transform.position - transform.position);
        float distSqr = dir.magnitude; 

        if (coolDown < 0 &&
            !Physics.Raycast(transform.position, dir, startUpwardDist, impassableMask)){
            playerStats.TakeDamage(4);
            coolDown = coolDownMax;
        }
        else{
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsTarget(){
        Vector3 dir =  (target.transform.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, startUpwardDist, impassableMask)){
            dir.y = 1f;

            if (hit.distance < useNavMeshDist){
                navMeshAgent.destination = target.transform.position;
                return;
            }
        }

        acculmulatedSpeed += dir * moveSpeed * Time.deltaTime;
        rb.velocity = acculmulatedSpeed;
    }

    void MoveTowardsPlayer(){
        Vector3 dir =  (player.transform.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, startUpwardDist, impassableMask)){
            dir.y = 1f;

            if (hit.distance < useNavMeshDist){
                navMeshAgent.destination = target.transform.position;
                return;
            }
        }

        acculmulatedSpeed += dir * moveSpeed * Time.deltaTime;
        rb.velocity = acculmulatedSpeed;
    }
}
