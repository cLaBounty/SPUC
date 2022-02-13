using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] int hp = 1;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] float attackSpeed = 1;
    [SerializeField] float attackPower = 1;
    [SerializeField] float defense = 0;

    //[Header("Debuging")]
    //[SerializeField] 
    GameObject target;

    public void TakeDamage (float damage) {
        hp -= Mathf.CeilToInt(damage - defense);
        KillEnemy();
    }

    void KillEnemy(){
        if (hp < 1){
            Destroy(gameObject);
        }
    }
}
