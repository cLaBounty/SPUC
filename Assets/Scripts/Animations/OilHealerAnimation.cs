using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilHealerAnimation : MonoBehaviour
{
    Animator animator;
    [SerializeField] DeployedOilHealer enemy;
    [SerializeField] float deathTimer = 2f;
    //[SerializeField] AudioSource SpewSFX;
    //[SerializeField] AudioSource DeathSFX;
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
                //DeathSFX.pitch = Random.Range(0.9f, 1.1f);
                //DeathSFX.Play();
            }

        }
        else if (enemy.state == Enemy.STATE.AGRO_OIL || enemy.state == Enemy.STATE.AGRO_PLAYER || enemy.state == Enemy.STATE.AGRO_DISTRACTION)//state < 4 && enemy.coolDown < 0){
            animator.SetInteger("State", 0);

        else if (enemy.state == Enemy.STATE.ATTACKING_OIL || enemy.state == Enemy.STATE.ATTACKING_PLAYER)
            animator.SetInteger("State", 1);
        
    }

    IEnumerator Disapear(){
        yield return new WaitForSeconds(deathTimer);
        Destroy(enemy.gameObject);
    }

    //public void AttackKeyFrame(){
    //    var inst = Instantiate(projectile);
    //    inst.transform.parent = null;
    //    inst.transform.position = projectileSpawnPoint.transform.position;

        //SpewSFX.pitch = Random.Range(0.9f, 1.1f);
        //SpewSFX.Play();

    //    enemy.DealDamage(inst);
    //}

    //public void EndAttackKeyFrame(){
    //   attacking = false;
    //}
}
