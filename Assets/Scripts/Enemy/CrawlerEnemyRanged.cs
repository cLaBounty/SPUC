using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrawlerEnemyRanged : Enemy
{
    [Header("Agro Vars")]
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

    [Header("Projectile")]
    [SerializeField]float projectileSpeed = 3f;
    [SerializeField]float steeringSpeed = 1f;
    [SerializeField]Vector2 randomSpawnTimerRange = Vector2.one;

    LayerMask impassableMask;

    new void Start()
    {
        base.Start();
        
        rb              = GetComponent<Rigidbody>();
        agroRangeSqr    = agroDistance * agroDistance;
        attackRangeSqr  = attackDistance * attackDistance;
        
        //set navmeshagent
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;

        //impassable mask
        impassableMask = LayerMask.GetMask("Impassible Terrain");
        StartCoroutine(RandomShot(Random.Range(randomSpawnTimerRange.x, randomSpawnTimerRange.y)));
    }

    private void Update() {
        base.Update();
        
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
            if (isDistracted && currentTargetDist > agroRangeSqr){
                isDistracted = false;
                state = STATE.AGRO_DISTRACTION;
            }

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
            Cell occupideCell = flowField.curFlowField.GetCellFromWorldPos(transform.position);
            Vector3 moveDirection;

            navMeshAgent.speed = 0;
            moveDirection = new Vector3(occupideCell.bestDirection.x, 0, occupideCell.bestDirection.y);
            Vector3 lookVector = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);

            acculmulatedSpeed *= damping;
            acculmulatedSpeed += moveDirection * moveSpeed * Time.fixedDeltaTime;
            acculmulatedSpeed = Vector3.ClampMagnitude(acculmulatedSpeed, maxMoveSpeed);
            rb.velocity = new Vector3(acculmulatedSpeed.x, rb.velocity.y, acculmulatedSpeed.z);
        }
        else{
            Debug.Log("Flow Field not Initialized");
        }
    }
    
    void MoveTowardsAttractItem() {
        if (target != null && currentTargetDist < attackRangeSqr){
            isDistracted = true;
            state = STATE.ATTACKING_OIL;
        }
        else{
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.destination = target.transform.position;
            rb.velocity = Vector3.zero;
            acculmulatedSpeed = Vector3.zero;

            Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
            transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
        }
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

    public void DealDamage(GameObject projectile){
        EnemyProjectile ep = projectile.GetComponent<EnemyProjectile>();

        if (isOil && target != null)
            ep.target = target;
        else if (!isOil && player != null)
            ep.target = player.gameObject;

        ep.moveDirection = (ep.target.transform.position - ep.transform.position).normalized;
        ep.steeringSpeed = steeringSpeed;
        ep.projectileSpeed = projectileSpeed;
        ep.damage = attackPower;

        coolDown = coolDownMax;

        //change state
        if (isDistracted && currentTargetDist < agroRangeSqr)
            state = STATE.ATTACKING_OIL;
        else if (currentPlayerDist < agroRangeSqr)
            state = STATE.AGRO_PLAYER;
    }

    void Stop(){
        rb.velocity = Vector3.zero;
        acculmulatedSpeed = Vector3.zero;
        navMeshAgent.speed = 0;
    }

    IEnumerator RandomShot(float time){
        yield return new WaitForSeconds(time);

        if (state == STATE.AGRO_PLAYER && player != null){
            Vector3 dir = player.transform.position - transform.position;
            if (!Physics.Raycast(transform.position, dir, dir.magnitude, impassableMask)){
                state = STATE.ATTACKING_PLAYER;
                coolDown = coolDownMax * 2;
            }
        }

        StartCoroutine(RandomShot(Random.Range(randomSpawnTimerRange.x, randomSpawnTimerRange.y)));
    }
}
