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
    Vector3 acculmulatedSpeed = Vector3.zero;

    float agroRangeSqr = 0;
    float attackRangeSqr = 0;
    public float coolDown = 0;
    public float coolDownMax = 2f;

    public float currentTargetDist = 0;
    float currentPlayerDist = 0;
    bool isOil;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        
        rb              = GetComponent<Rigidbody>();
        agroRangeSqr    = agroDistance * agroDistance;
        attackRangeSqr  = attackDistance * attackDistance;
        
        //set navmeshagent
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    // Update is called once per frame
    private void Update() {
        //update distances
        currentTargetDist = (target.transform.position - transform.position).sqrMagnitude;
        currentPlayerDist = (player.transform.position - transform.position).sqrMagnitude;

        if (player == null && ((state == STATE.AGRO_PLAYER) || (state == STATE.ATTACKING_PLAYER))){
            state = STATE.AGRO_OIL;
        }

        switch(state){
            case STATE.AGRO_DISTRACTION:    MoveTowardsAttractItem(); break;
            case STATE.AGRO_OIL:            MoveTowardsTarget(); break;
            case STATE.AGRO_PLAYER:         MoveTowardsPlayer(); break;
            case STATE.ATTACKING_OIL:       AttackOilDrill(); break;
            case STATE.ATTACKING_PLAYER:    AttackPlayer(); break;
            case STATE.DEAD:                Stop(); break;
        }

        if (coolDown >= 0)
            coolDown -= Time.deltaTime;
    }

    void AttackPlayer(){
        //stop
        Stop();
        isOil = false;

        if (coolDown < 0) {
            //exit condition first
            if (currentPlayerDist > attackRangeSqr && currentPlayerDist < agroRangeSqr)
                state = STATE.AGRO_PLAYER;
            else if (currentPlayerDist > agroRangeSqr)
                state = STATE.AGRO_OIL;
        }
        
        
        Vector3 lookVector = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
        transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
    }

    void AttackOilDrill(){
        Stop();

        isOil = true;

        if (coolDown < 0) {
            //exit condition first
            if (currentTargetDist > agroRangeSqr)
                state = STATE.AGRO_OIL;
        }

        Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
    }

    void MoveTowardsTarget(){
        //exit condition
        if (currentPlayerDist < agroRangeSqr)
            state = STATE.AGRO_PLAYER;

        else if (target != null && currentTargetDist < attackRangeSqr)
            state = STATE.ATTACKING_OIL;

        else if (flowField != null && flowField.initialized){
            //Debug.Log("Target");
            Cell occupideCell = flowField.curFlowField.GetCellFromWorldPos(transform.position);
            Vector3 moveDirection;

            if (occupideCell.cost == 255){
                navMeshAgent.speed = moveSpeed;
                navMeshAgent.destination = target.transform.position;
                rb.velocity = Vector3.zero;
                acculmulatedSpeed = Vector3.zero;
                return;
            }
            else{
                navMeshAgent.speed = 0;
                moveDirection = new Vector3(occupideCell.bestDirection.x, 0, occupideCell.bestDirection.y);
            }

            acculmulatedSpeed *= damping;
            acculmulatedSpeed += moveDirection * moveSpeed * Time.fixedDeltaTime;
            acculmulatedSpeed = Vector3.ClampMagnitude(acculmulatedSpeed, maxMoveSpeed);
            rb.velocity = new Vector3(acculmulatedSpeed.x, rb.velocity.y, acculmulatedSpeed.z);

            Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
            transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
        }
        else{
            Debug.Log("Flow Field not Initialized");
        }
    }
    
    void MoveTowardsAttractItem() {
        navMeshAgent.speed = 0;
        
        Vector3 moveDirection = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        acculmulatedSpeed *= damping;
        acculmulatedSpeed += moveDirection * moveSpeed * Time.fixedDeltaTime;
        acculmulatedSpeed = Vector3.ClampMagnitude(acculmulatedSpeed, maxMoveSpeed);
        rb.velocity = new Vector3(acculmulatedSpeed.x, rb.velocity.y, acculmulatedSpeed.z);

        Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
    }

    void MoveTowardsPlayer(){
        //exit condition
        if (currentPlayerDist > agroRangeSqr)
            state = STATE.AGRO_OIL;
        
        else if (currentPlayerDist < attackRangeSqr)
            state = STATE.ATTACKING_PLAYER;

        else{
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.destination = player.transform.position;
            rb.velocity = Vector3.zero;
            acculmulatedSpeed = Vector3.zero;

            Vector3 lookVector = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
            transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
        }
    }

    public void DealDamage(){
        if (isOil && target != null){
            target.GetComponent<OilDrill>()?.TakeDamage(attackPower);
        }
        else if (!isOil && player != null && currentPlayerDist < attackRangeSqr){
            playerStats.TakeDamage(attackPower);
        }

        coolDown = coolDownMax;
    }

    void Stop(){
        rb.velocity = Vector3.zero;
        acculmulatedSpeed = Vector3.zero;
        navMeshAgent.speed = 0;
    }
}
