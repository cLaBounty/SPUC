using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//[RequireComponent(typeof(Rigidbody))] 
public class DeployedEnemyPuncher : Enemy
{
    [Header("Agro Vars")]
    [SerializeField] float attackOilEnemyDist = 4f;
    [SerializeField] float damping = 0.98f;
    [SerializeField] float searchRadius = 6f;
    [SerializeField] LayerMask searchFields;

    private DeployedStatus status;

    NavMeshAgent navMeshAgent;
    Rigidbody rb;
    Vector3 acculmulatedSpeed = Vector3.zero;

    float attackRangeEnemySqr = 0;
    public float coolDown = 0;
    public float coolDownMax = 0.05f;

    public float currentTargetDist = 0;
    float currentPlayerDist = 0;
    bool isOil;

    // Start is called before the first frame update
    new void Start()
    {
        status = GetComponent<DeployedStatus>();
        
        SetHealth(currentHealth);
        healthBar.transform.gameObject.SetActive(false);
        
        rb              = GetComponent<Rigidbody>();
        attackRangeEnemySqr = attackOilEnemyDist * attackOilEnemyDist;
        
        //set navmeshagent
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        StartCoroutine(FindNewFoe());
    }

    // Update is called once per frame
    private void Update() {
        if (!IsActive()) { return; }

        if (target == null){
            FindFoe();
            Stop();
            return;
        }

        //update distances
        currentTargetDist = (target.transform.position - transform.position).sqrMagnitude;

        if (state == STATE.AGRO_DISTRACTION || state == STATE.ATTACKING_PLAYER || state == STATE.AGRO_PLAYER)
            state = STATE.AGRO_OIL;

        switch(state){
            case STATE.AGRO_OIL:            MoveTowardsTarget(); break;
            case STATE.ATTACKING_OIL:       AttackOilDrill(); break;
            case STATE.DEAD:                Stop(); break;
        }

        if (coolDown >= 0)
            coolDown -= Time.deltaTime;
    }

    private bool IsActive() {
        return status.isActive;
    }

    void AttackOilDrill(){
        Stop();

        isOil = true;

        if (coolDown < 0) {
            //exit condition first
            if (currentTargetDist > attackRangeEnemySqr)
                state = STATE.AGRO_OIL;
            else{
                Enemy enemy = target.GetComponent<Enemy>();

                if (enemy != null)
                    enemy.TakeDamage(attackPower);
                else 
                   state = STATE.AGRO_OIL; 
            }

            coolDown = coolDownMax;
        }

        Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
    }

    void MoveTowardsTarget(){
        //exit condition
        if (target != null && currentTargetDist < attackRangeEnemySqr)
            state = STATE.ATTACKING_OIL;

        navMeshAgent.speed = moveSpeed;
        navMeshAgent.destination = target.transform.position;
        rb.velocity = Vector3.zero;
        //acculmulatedSpeed = Vector3.zero;
        Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
    }

    void Stop(){
        rb.velocity = Vector3.zero;
        acculmulatedSpeed = Vector3.zero;
        navMeshAgent.speed = 0;
    }

    new void OnDestroy() {
        //do nothing
    }

    void FindFoe(){
        Collider[]colliders =  Physics.OverlapSphere(transform.position, searchRadius, searchFields);
        if (colliders.Length > 0){
            float smallestDist = (transform.position - colliders[0].transform.position).sqrMagnitude * 2;
            int index = -1;

            for (int i = 0; i < colliders.Length; ++i){
                float tempDist = (transform.position - colliders[i].transform.position).sqrMagnitude;
                Enemy enemy = colliders[i].GetComponent<Enemy>();
                if (tempDist < smallestDist && enemy.isEnemy && !enemy.isFlying){
                    smallestDist = tempDist;
                    index = i;
                }
            }


            if (index != -1)
                target = colliders[index].gameObject;
        }

        state = STATE.AGRO_OIL;
    }

    IEnumerator FindNewFoe(){
        yield return new WaitForSeconds(5f);
        FindFoe();
        StartCoroutine(FindNewFoe());
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
