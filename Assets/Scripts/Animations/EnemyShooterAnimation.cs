using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterAnimation : MonoBehaviour
{
    [SerializeField] DeployedEnemyShooter helper;
    [SerializeField] float deathTimer = 2f;
    [SerializeField] AudioSource ShootSFX;
    [SerializeField] AudioSource DeathSFX;

    Animator animator;

    int state;
    bool attacking = false;
    bool dying = false;
    
    void Start(){
        animator = GetComponent<Animator>();
    }

    void LateUpdate() {
        if (helper.state == Enemy.STATE.DEAD ){
            animator.SetInteger("State", 2);
            
            if (!dying){
                dying = true;
                StartCoroutine(Disapear());
                DeathSFX.pitch = Random.Range(1.9f, 2.1f);
                DeathSFX.Play();
            }
        }
        else if (helper.target == null)
            animator.SetInteger("State", 3);
        
        else if (helper.state == Enemy.STATE.AGRO_OIL || helper.state == Enemy.STATE.AGRO_PLAYER || helper.state == Enemy.STATE.AGRO_DISTRACTION)
            animator.SetInteger("State", 3);
        
        else if (helper.state == Enemy.STATE.ATTACKING_OIL || helper.state == Enemy.STATE.ATTACKING_PLAYER){
            animator.SetInteger("State", 1);
        }
    }

    public void ShootFX(){
        ShootSFX.pitch = Random.Range(0.9f, 1.1f);
        ShootSFX.Play();
    }

    IEnumerator Disapear(){
        yield return new WaitForSeconds(deathTimer);
        Destroy(helper.gameObject);
    }
}
