using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyerMeleeAnimation : MonoBehaviour
{
    [SerializeField] FlyingEnemyMelee enemy;
    [SerializeField] AudioSource FlapSFX;
    [SerializeField] AudioSource SpewSFX;
    [SerializeField] AudioSource DeathSFX;

    Animator animator;

    int state;
    bool attacking = false;
    bool dying = false;
    
    void Start(){
        animator = GetComponent<Animator>();
    }

    void LateUpdate() {
        if (enemy.state == Enemy.STATE.DEAD || enemy.currentHealth <= 0){
            animator.SetInteger("State", 2);

            if (!dying){
                dying = true;
                DeathSFX.pitch = Random.Range(1.9f, 2.1f);
                DeathSFX.Play();
            }
        }
        else if (enemy.state == Enemy.STATE.AGRO_OIL || enemy.state == Enemy.STATE.AGRO_PLAYER || enemy.state == Enemy.STATE.AGRO_DISTRACTION)
            animator.SetInteger("State", 0);
        else if (enemy.state == Enemy.STATE.ATTACKING_OIL || enemy.state == Enemy.STATE.ATTACKING_PLAYER)
            animator.SetInteger("State", 1);
    }

    public void AttackKeyFrame(){
        SpewSFX.pitch = Random.Range(0.9f, 1.1f);
        SpewSFX.Play();
        enemy.DealDamage();
    }

    public void Flap(){
        FlapSFX.pitch = Random.Range(0.9f, 1.1f);
        FlapSFX.Play();
    }

    public void EndAttackKeyFrame(){
        attacking = false;
    }

    public void Disapear(){
        LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
        if (levelManager != null) levelManager.EnemyKilled();
        Destroy(enemy.gameObject);
    }
}
