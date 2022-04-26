using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilHealerAnimation : MonoBehaviour
{
    [SerializeField] DeployedOilHealer enemy;
    [SerializeField] float deathTimer = 2f;

    Animator animator;

    int state;
    bool attacking = false;
    bool dying = false;
    
    void Start(){
        animator = GetComponent<Animator>();
    }

    void LateUpdate() {
        if (enemy.state == Enemy.STATE.DEAD ){
            animator.SetInteger("State", 2);
            
            if (!dying){
                dying = true;
                StartCoroutine(Disapear());
            }
        }
        else if (enemy.state == Enemy.STATE.AGRO_OIL || enemy.state == Enemy.STATE.AGRO_PLAYER || enemy.state == Enemy.STATE.AGRO_DISTRACTION)
            animator.SetInteger("State", 0);
        else if (enemy.state == Enemy.STATE.ATTACKING_OIL || enemy.state == Enemy.STATE.ATTACKING_PLAYER)
            animator.SetInteger("State", 1);
    }

    IEnumerator Disapear(){
        yield return new WaitForSeconds(deathTimer);
        Destroy(enemy.gameObject);
    }
}
