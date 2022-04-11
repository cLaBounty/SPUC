using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyerAnimation1 : MonoBehaviour
{
    Animator animator;
    [SerializeField] FlyingEnemy enemy;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectileSpawnPoint;
    int state;

    bool attacking = false;
    bool dying = false;
    
    void Start(){
        animator = GetComponent<Animator>();
    }

    void LateUpdate() {

        if (enemy.state == Enemy.STATE.DEAD )
            animator.SetInteger("State", 2);

        else if (enemy.state == Enemy.STATE.AGRO_OIL || enemy.state == Enemy.STATE.AGRO_PLAYER)//state < 4 && enemy.coolDown < 0){
            animator.SetInteger("State", 0);
        

        else if (enemy.state == Enemy.STATE.ATTACKING_OIL || enemy.state == Enemy.STATE.ATTACKING_PLAYER)
            animator.SetInteger("State", 1);
        
    }

    public void AttackKeyFrame(){
        var inst = Instantiate(projectile);
        inst.transform.parent = null;
        inst.transform.position = projectileSpawnPoint.transform.position;
        enemy.DealDamage(inst);
    }

    public void EndAttackKeyFrame(){
        attacking = false;
    }

    public void Disapear(){
        Destroy(enemy.gameObject);
    }
}