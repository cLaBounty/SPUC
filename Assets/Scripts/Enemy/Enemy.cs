using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float maxMoveSpeed = 1f;
    [SerializeField] protected float attackSpeed = 1f;
    public float attackPower = 1f;
    public float defense = 0;

    //[Header("Debuging")]
    //[SerializeField] 
    public GameObject target;
    public GridController flowField = null;
    public PlayerMovement player = null;
    public Player playerStats = null;
    public LevelManager levelManager = null;

    public enum STATE {
        AGRO_OIL = 0,
        AGRO_DISTRACTION,
        AGRO_PLAYER,
        ATTACKING_OIL,
        ATTACKING_PLAYER,
        DEAD,
    }

    public STATE state = STATE.AGRO_OIL;

    public HealthBar healthBar;
    public float currentHealth = 100;

    protected void Start() {
        SetHealth(100f); // ToDo: remove when health is set in level manager
        healthBar.transform.gameObject.SetActive(false);
    }

    public void TakeDamage (float damage) {
        currentHealth -= damage - defense;
        healthBar.transform.gameObject.SetActive(true);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) {
            healthBar.transform.gameObject.SetActive(false);

            // Kill Enemy
            Collider collider = GetComponent<Collider>();
            if (collider != null ) collider.enabled = false;
            state = STATE.DEAD;
        }
    }

    public void SetHealth(float value) {
        currentHealth = value;
        healthBar.SetMaxHealth(value);
    }

    private void OnDestroy() {
        if (levelManager != null) levelManager.EnemyKilled();
    }
}
