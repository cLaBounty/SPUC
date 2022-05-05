using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrawlerAnimation : MonoBehaviour
{
    [SerializeField] CrawlerEnemy enemy;
    [SerializeField] AudioSource SpewSFX;
    [SerializeField] AudioSource DeathSFX;

    Animator animator;

    int state;
    bool attacking = false;
    bool dying = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        state = (int)enemy.state;
    }

    void LateUpdate() {
        state = (int)enemy.state;

        if (enemy.state == Enemy.STATE.DEAD || enemy.currentHealth <= 0){
            int ran = Random.Range(0, 2);
            animator.SetInteger("Variation", ran);
            animator.SetInteger("State", 4);

            if (!dying){
                dying = true;
                DeathSFX.pitch = Random.Range(0.9f, 1.1f);
                DeathSFX.Play();
            }
        }
        else if (state < 3){
            int ran = Random.Range(0, 2);
            animator.SetInteger("Variation", ran);
            animator.SetInteger("State", 1);
        }
        else if (state < 5 && enemy.coolDown < 0){
            int ran = Random.Range(0, 2);
            animator.SetInteger("Variation", ran);
            animator.SetInteger("State", 3);
        }
    }

    public void AttackKeyFrame(){
        SpewSFX.pitch = Random.Range(0.9f, 1.1f);
        SpewSFX.Play();
        enemy.DealDamage();
    }

    public void EndAttackKeyFrame(){
        attacking = false;
        animator.SetInteger("Variation", 2);
    }

    public void Disapear(){
        LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
        if (levelManager != null) levelManager.EnemyKilled();
        Destroy(enemy.gameObject);
    }
}
