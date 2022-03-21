using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected int hp = 1;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float maxMoveSpeed = 1f;
    [SerializeField] protected float attackSpeed = 1;
    [SerializeField] protected float attackPower = 1;
    [SerializeField] protected float defense = 0;

    //[Header("Debuging")]
    //[SerializeField] 
    public OilDrill target;
    public GridController flowField = null;
    public PlayerMovement player = null;
    public Player playerStats = null;
    public LevelManager levelManager = null;

    public enum STATE {
        AGRO_OIL = 0,
        AGRO_PLAYER,
        ATTACKING_OIL,
        ATTACKING_PLAYER,
        DEAD,
    }

    public STATE state = STATE.AGRO_OIL;

    public void TakeDamage (float damage) {
        hp -= Mathf.CeilToInt(damage - defense);
        KillEnemy();
    }

    void KillEnemy(){
        if (hp <= 0){
            Collider collider = GetComponent<Collider>();
            if (collider != null ) collider.enabled = false;

            state = STATE.DEAD;//Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        if (levelManager != null) levelManager.EnemyKilled();
    }
}
