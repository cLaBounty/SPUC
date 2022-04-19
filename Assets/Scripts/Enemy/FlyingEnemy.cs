using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody))] 
public class FlyingEnemy : Enemy
{
    [Header("Flying Vars")]
    [SerializeField]float agroDistance = 0f;
    [SerializeField]float attackDistance = 1f;
    [SerializeField]float attackDistanceOil = 8f;
    [SerializeField]float damping = 0.98f;
    [SerializeField]float maxFlyHeight = 10f;
    [SerializeField]float minFlyHeight = 3f;
    [SerializeField]float startUpwardDist = 20f;
    [SerializeField]float useNavMeshDist = 3f;

    [SerializeField] LayerMask groundMask;
    [SerializeField] Vector3 flyUpOffset = Vector3.up;

    public float currentTargetDist = 0;

    [Header("Projectile")]
    [SerializeField]float projectileSpeed = 3f;
    [SerializeField]float steeringSpeed = 1f;
    [SerializeField]Vector2 randomSpawnTimerRange = Vector2.one;

    NavMeshAgent navMeshAgent;
    Rigidbody rb;
    BoxCollider bc;
    //GridController flowField = null;
    //PlayerMovement player = null;
    //Player playerStats = null;
    Vector3 acculmulatedSpeed = Vector3.zero;

    float agroRangeSqr = 0;
    float attackRangeSqr = 0;
    float attackDistanceOilSqr = 0;
    float coolDown = 0;
    float coolDownMax = 2f;
    
    int impassableMask;
    float currentPlayerDist = 0;
    bool isOil;

    // Start is called before the first frame update
    void Start()
    {
        rb              = GetComponent<Rigidbody>();
        agroRangeSqr    = agroDistance * agroDistance;
        attackRangeSqr  = attackDistance * attackDistance;
        
        //set navmeshagent
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.enabled = false;

        bc = GetComponent<BoxCollider>();

        //impassable mask
        impassableMask = LayerMask.GetMask("Impassible Terrain");
        attackDistanceOilSqr = attackDistanceOil * attackDistanceOil;
        StartCoroutine(RandomShot(Random.Range(randomSpawnTimerRange.x, randomSpawnTimerRange.y)));
    }

    // Update is called once per frame
    private new void Update() {
        base.Update();
        //update distances
        currentTargetDist = (target.transform.position - transform.position).sqrMagnitude;
        currentPlayerDist = (player.transform.position - transform.position).sqrMagnitude;

        if (player == null && ((state == STATE.AGRO_PLAYER) || (state == STATE.ATTACKING_PLAYER))){
            state = STATE.AGRO_OIL;
        }

        switch(state){
            case STATE.AGRO_OIL:            MoveTowardsTarget(); break;
            case STATE.AGRO_PLAYER:         MoveTowardsPlayer(); break;
            case STATE.ATTACKING_OIL:       AttackOilDrill(); break;
            case STATE.ATTACKING_PLAYER:    AttackPlayer(); break;
            case STATE.DEAD:                rb.useGravity = true; break;
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
            if (currentPlayerDist < agroRangeSqr)
                state = STATE.AGRO_PLAYER;
            else if (currentTargetDist > agroRangeSqr)
                state = STATE.AGRO_OIL;
        }

        Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
    }

    void MoveTowardsTarget(){
        //state change
        if (currentPlayerDist < agroRangeSqr)
            state = STATE.AGRO_PLAYER;

        else if (target != null && currentTargetDist < attackRangeSqr)
            state = STATE.ATTACKING_OIL;

        else{
            Vector3 dir =  (target.transform.position - transform.position).normalized;
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, 1000, groundMask);
            bool toClosetoSolid = Physics.CheckBox(transform.position + bc.center, bc.size * 2.5f, Quaternion.identity, impassableMask);

            if (Physics.Raycast(transform.position, dir, startUpwardDist, impassableMask) || hit.distance < minFlyHeight || toClosetoSolid){
                dir.y = 1f;

                if (toClosetoSolid) transform.position += flyUpOffset * Time.deltaTime;

                Debug.Log("Flying Up");
            }
            else if (hit.distance > maxFlyHeight){
                dir.y = -1f;
            }

            navMeshAgent.speed = 0;
            acculmulatedSpeed += dir * moveSpeed * Time.deltaTime;
            acculmulatedSpeed = Vector3.ClampMagnitude(acculmulatedSpeed, maxMoveSpeed);
            Debug.Log(acculmulatedSpeed);
            rb.velocity = acculmulatedSpeed;

            Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
            transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
        }
    }

    void MoveTowardsPlayer(){
        //state change
        if (currentPlayerDist > agroRangeSqr)
            state = STATE.AGRO_OIL;

        else if (currentPlayerDist < attackRangeSqr)
            state = STATE.ATTACKING_PLAYER;

        else{
            Vector3 dir =  (player.transform.position - transform.position).normalized;
            RaycastHit hit;

            Physics.Raycast(transform.position, Vector3.down, out hit, 1000, groundMask);
            bool toClosetoSolid = Physics.CheckBox(transform.position + bc.center, bc.size * 2.5f, Quaternion.identity, impassableMask);

            if (Physics.Raycast(transform.position, dir, startUpwardDist, impassableMask) || hit.distance < minFlyHeight || toClosetoSolid){

                if (toClosetoSolid) transform.position += flyUpOffset * Time.deltaTime;
                dir.y = 1f;
                Debug.Log("Flying Up");
            }
            else if (hit.distance > maxFlyHeight){
                dir.y = -1f;
            }

            navMeshAgent.speed = 0;
            acculmulatedSpeed += dir * moveSpeed * Time.deltaTime;
            acculmulatedSpeed = Vector3.ClampMagnitude(acculmulatedSpeed, maxMoveSpeed);
            rb.velocity = acculmulatedSpeed;

            Vector3 lookVector = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
            transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
        }
    }

    public void DealDamage(GameObject projectile){
        EnemyProjectile ep = projectile.GetComponent<EnemyProjectile>();

        if (isOil && target != null){
            ep.target = target;
        }
        else if (!isOil && player != null){
            ep.target = player.gameObject;
        }

        ep.moveDirection = (ep.target.transform.position - ep.transform.position).normalized;
        ep.steeringSpeed = steeringSpeed;
        ep.projectileSpeed = projectileSpeed;
        ep.damage = attackPower;

        coolDown = coolDownMax;

        //change state
        if (currentPlayerDist > attackRangeSqr && currentPlayerDist < agroRangeSqr)
            state = STATE.AGRO_PLAYER;
        else if (currentPlayerDist > agroRangeSqr)
            state = STATE.AGRO_OIL;
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
