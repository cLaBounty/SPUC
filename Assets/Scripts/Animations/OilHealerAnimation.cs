using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilHealerAnimation : MonoBehaviour
{
    [SerializeField] DeployedOilHealer enemy;
    [SerializeField] float deathTimer = 2f;
    [SerializeField] AudioSource HealSFX;
    [SerializeField] AudioSource DeathSFX;

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
                DeathSFX.pitch = Random.Range(1.9f, 2.1f);
                DeathSFX.Play();
            }
        }
        else if (enemy.state == Enemy.STATE.AGRO_OIL || enemy.state == Enemy.STATE.AGRO_PLAYER || enemy.state == Enemy.STATE.AGRO_DISTRACTION){
            animator.SetInteger("State", 0);
            if (HealSFX.isPlaying) HealSFX.Stop();
        }

        else if (enemy.state == Enemy.STATE.ATTACKING_OIL || enemy.state == Enemy.STATE.ATTACKING_PLAYER){
            animator.SetInteger("State", 1);
            if (!HealSFX.isPlaying){
                HealSFX.pitch = Random.Range(0.9f, 1.1f);
                HealSFX.Play();
            }
        }
    }

    IEnumerator Disapear(){
        yield return new WaitForSeconds(deathTimer);
        Destroy(enemy.gameObject);
    }
}
