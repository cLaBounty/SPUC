using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody))] 
public class OilDrillHealer : Enemy
{
    [Header("Agro Vars")]
    [SerializeField] float attackOilDistance = 7f;
    [SerializeField] float damping = 0.98f;
    [SerializeField] float healAmmount = 1;

    [Header("Collisions")]
    [SerializeField] Vector3 BoxColldierDimensions;
    [SerializeField] Vector3 BoxColldierCenter;
    [SerializeField] LayerMask ProjecileLayer;

    NavMeshAgent navMeshAgent;
    Rigidbody rb;
    Vector3 acculmulatedSpeed = Vector3.zero;
    OilDrill oildDril;

    float agroRangeSqr = 0;
    float attackRangeOilSqr = 0;

    [Header("Attack Vars")]
    public float coolDown = 0;
    public float coolDownMax = 0.05f;

    public float currentTargetDist = 0;
    float currentPlayerDist = 0;
    bool isOil;

    DeployedStatus deployedStatus;

    // Start is called before the first frame update
    new void Start()
    {
        //base.Start();
        deployedStatus = GetComponent<DeployedStatus>();

        SetHealth(currentHealth);
        healthBar.transform.gameObject.SetActive(false);
        
        rb              = GetComponent<Rigidbody>();
        attackRangeOilSqr = attackOilDistance * attackOilDistance;
        
        //set navmeshagent
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        target = GameObject.FindObjectOfType<OilDrill>().gameObject;
        oildDril = target.GetComponent<OilDrill>();
    }

    // Update is called once per frame
    private void Update() {
        if (!deployedStatus.isActive) return;

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
        
        CheckForProjectiles();
    }

    void AttackOilDrill(){
        Stop();

        isOil = true;

        if (coolDown < 0) {
            //exit condition first
            if (currentTargetDist > attackRangeOilSqr)
                state = STATE.AGRO_OIL;
            else{
                if (oildDril == null) { oildDril = target.GetComponent<OilDrill>(); return;} 

                oildDril.GainHealth(healAmmount);
            }

            coolDown = coolDownMax;
        }

        Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
    }

    void MoveTowardsTarget(){
        //exit condition
        if (target != null && currentTargetDist < attackRangeOilSqr)
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
                Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
                transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
                return;
            }
            else{
                navMeshAgent.speed = 0;
                moveDirection = new Vector3(occupideCell.bestDirection.x, 0, occupideCell.bestDirection.y);
                Vector3 lookVector = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
            }

            acculmulatedSpeed *= damping;
            acculmulatedSpeed += moveDirection * moveSpeed * Time.fixedDeltaTime;
            acculmulatedSpeed = Vector3.ClampMagnitude(acculmulatedSpeed, maxMoveSpeed);
            rb.velocity = new Vector3(acculmulatedSpeed.x, rb.velocity.y, acculmulatedSpeed.z);
        }
        else{
            Debug.Log("Flow Field not Initialized");
            flowField = GameObject.FindObjectOfType<GridController>();
        }
    }

    void Stop(){
        rb.velocity = Vector3.zero;
        acculmulatedSpeed = Vector3.zero;
        navMeshAgent.speed = 0;
    }

    new void OnDestroy() {
        //do nothing
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + BoxColldierCenter, BoxColldierDimensions);
    }

    void CheckForProjectiles(){
        Collider[] col = Physics.OverlapBox(transform.position, BoxColldierDimensions, Quaternion.identity, ProjecileLayer);

        if (col.Length > 0){
            EnemyProjectile ep = col[0].gameObject.GetComponent<EnemyProjectile>();

            if (ep != null){
                TakeDamage(ep.damage);
                Destroy(col[0].gameObject);
            }
        }
    }
}
