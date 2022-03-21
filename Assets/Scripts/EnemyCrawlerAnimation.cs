using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrawlerAnimation : MonoBehaviour
{
    Animator animator;
    [SerializeField] CrawlerEnemy enemy;
    int state;

    bool attacking = false;
    bool dying = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        state = (int)enemy.state;
    }

    // Update is called once per frame
    void LateUpdate() {
        state = (int)enemy.state;

        animator.SetInteger("State", state);
        if (state < 2){
            int ran = Random.Range(0, 2);
            animator.SetInteger("Variation", ran);
        }

        else if (state < 4 && enemy.coolDown < 0){
            int ran = Random.Range(0, 2);
            animator.SetInteger("Variation", ran);
        }

        else {
            int ran = Random.Range(0, 2);
            animator.SetInteger("Variation", ran);
        }
    }

    public void AttackKeyFrame(){
        enemy.DealDamage();
    }

    public void EndAttackKeyFrame(){
        attacking = false;
        animator.SetInteger("Variation", 2);
    }

    public void Disapear(){
        Destroy(enemy.gameObject);
    }
}
