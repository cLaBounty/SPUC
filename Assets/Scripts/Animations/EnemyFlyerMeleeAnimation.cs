using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyerMeleeAnimation : MonoBehaviour
{
    Animator animator;
    [SerializeField] FlyingEnemyMelee enemy;
    int state;

    bool attacking = false;
    bool dying = false;
    
    void Start(){
        animator = GetComponent<Animator>();
    }

    void LateUpdate() {

        if (enemy.state == Enemy.STATE.DEAD )
            animator.SetInteger("State", 2);

        else if (enemy.state == Enemy.STATE.AGRO_OIL || enemy.state == Enemy.STATE.AGRO_PLAYER || enemy.state == Enemy.STATE.AGRO_DISTRACTION)//state < 4 && enemy.coolDown < 0){
            animator.SetInteger("State", 0);
        

        else if (enemy.state == Enemy.STATE.ATTACKING_OIL || enemy.state == Enemy.STATE.ATTACKING_PLAYER)
            animator.SetInteger("State", 1);
        
    }

    public void AttackKeyFrame(){
        enemy.DealDamage();
    }

    public void EndAttackKeyFrame(){
        attacking = false;
    }

    public void Disapear(){
        Destroy(enemy.gameObject);
    }
}
