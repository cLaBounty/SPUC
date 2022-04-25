using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody))] 
public class DeployedEnemyShooter : Enemy
{
    [Header("Agro Vars")]
    [SerializeField] float attackEnemyDist = 4f;
    [SerializeField] float damping = 0.98f;
    [SerializeField] float searchRadius = 6f;
    [SerializeField] LayerMask searchFields;

    [Header("Collisions")]
    [SerializeField] Vector3 BoxColldierDimensions;
    [SerializeField] Vector3 BoxColldierCenter;
    [SerializeField] LayerMask ProjecileLayer;

    //NavMeshAgent navMeshAgent;
    Rigidbody rb;
    Vector3 acculmulatedSpeed = Vector3.zero;

    float attackRangeEnemySqr = 0;

    [Header("Attacks")]
    public float coolDown = 0;
    public float coolDownMax = 0.05f;

    public float currentTargetDist = 0;
    float currentPlayerDist = 0;
    bool isOil;

    Player player;
    DeployedStatus status;

    // Start is called before the first frame update
    new void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        status = GetComponent<DeployedStatus>();

        SetHealth(maxHealth + (player.maxHealth - player.initialHealth));
        healthBar.transform.gameObject.SetActive(false);
        
        rb              = GetComponent<Rigidbody>();
        attackRangeEnemySqr = attackEnemyDist * attackEnemyDist;
        
        //set navmeshagent
        //navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        //navMeshAgent.speed = moveSpeed;
        StartCoroutine(FindNewFoe());
    }

    // Update is called once per frame
    private void Update() {
        if (!status.isActive) return;
        
        if (target == null){
            FindFoe();
            return;
        }

        //update distances
        currentTargetDist = (target.transform.position - transform.position).sqrMagnitude;

        if (state == STATE.AGRO_DISTRACTION || state == STATE.ATTACKING_PLAYER || state == STATE.AGRO_PLAYER || state == STATE.AGRO_OIL)
            state = STATE.ATTACKING_OIL;

        switch(state){
            case STATE.ATTACKING_OIL:       AttackOilDrill(); break;
            case STATE.DEAD:                rb.useGravity = true; break;
        }

        if (coolDown >= 0)
            coolDown -= Time.deltaTime;
        
        CheckForProjectiles();
    }


    void AttackOilDrill(){
        //Stop();

        if (coolDown < 0) {
            Enemy enemy = target.GetComponent<Enemy>();

            if (enemy == null)
                return;
                
            enemy.TakeDamage(player.damageMultiplier * attackPower, true);

            coolDown = coolDownMax;
        }

        if (transform.position.y < target.transform.position.y){
            acculmulatedSpeed += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
        else{
            acculmulatedSpeed -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }

        if (acculmulatedSpeed.sqrMagnitude > maxMoveSpeed * maxMoveSpeed)
            acculmulatedSpeed = acculmulatedSpeed.normalized * maxMoveSpeed;
        
        rb.velocity = acculmulatedSpeed;

        Vector3 lookVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
    }

    void Stop(){
        rb.velocity = Vector3.zero;
        acculmulatedSpeed = Vector3.zero;
        //navMeshAgent.speed = 0;
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
                if (tempDist < smallestDist && enemy.isEnemy && enemy.isFlying){
                    smallestDist = tempDist;
                    index = i;
                }
            }

            if (index != -1)
                target = colliders[index].gameObject;
        }

        state = STATE.ATTACKING_OIL;
    }

    IEnumerator FindNewFoe(){
        yield return new WaitForSeconds(5f);
        FindFoe();
        StartCoroutine(FindNewFoe());
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + BoxColldierCenter, BoxColldierDimensions);
    }

    void CheckForProjectiles(){
        Collider[] col = Physics.OverlapBox(transform.position, BoxColldierDimensions, Quaternion.identity, ProjecileLayer);

        if (col.Length > 0){
            EnemyProjectile ep = col[0].gameObject.GetComponent<EnemyProjectile>();

            if (ep != null){
                float damage = Mathf.Max(1f, ep.damage - player.defense);
                TakeDamage(damage);
                Destroy(col[0].gameObject);
            }
        }
    }
}
