using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
     public enum STATE {
        AGRO_OIL = 0,
        AGRO_DISTRACTION,
        AGRO_PLAYER,
        ATTACKING_OIL,
        ATTACKING_PLAYER,
        DEAD,
    }

    [System.Serializable]
    public struct ItemDrop{
        public GameObject item;
        public float chance;
    }

    [Header("Enemy Stats")]
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float maxMoveSpeed = 1f;
    [SerializeField] protected float attackSpeed = 1f;
    public float attackPower = 1f;
    public float defense = 0;
    public bool isEnemy = true;
    public bool isFlying = true;

    [HideInInspector]public GameObject target;
    [HideInInspector]public GridController flowField = null;
    [HideInInspector]public PlayerMovement player = null;
    [HideInInspector]public Player playerStats = null;
    [HideInInspector]public LevelManager levelManager = null;

    public STATE state = STATE.AGRO_OIL;

    public HealthBar healthBar;
    public float currentHealth;
    public float maxHealth = 100f;

    [Header("Drops (Organize in Order Please)")]
    [SerializeField] ItemDrop[] itemDrops;

    bool firstSetHealth = false; 
    public bool isDistracted = false;

    protected void Start() {
        SetHealth(maxHealth);
        healthBar.transform.gameObject.SetActive(false);
    }

    protected void Update() {
        CheckDeadState(); // Fixes issue of unkillable enemies
    }

    public void TakeDamage(float amount, bool ignoreSound = false) {
        if (state == STATE.DEAD) return;

        float damage = Mathf.Max(1f, amount - defense);
        if (damage > currentHealth) {
            Player.DamageDealt += currentHealth;
            currentHealth = 0;
        } else {
            Player.DamageDealt += damage;
            currentHealth -= damage;
        }
        healthBar.transform.gameObject.SetActive(true);
        healthBar.SetHealth(currentHealth);

        if (!ignoreSound) SFXManager.instance.Play("Enemy Hurt", 1.4f, 1.7f);

        if (currentHealth <= 0) {
            healthBar.transform.gameObject.SetActive(false);

            // Kill Enemy
            Collider collider = GetComponent<Collider>();
            if (collider != null ) collider.enabled = false;
            state = STATE.DEAD;
            Player.EnemiesKilled += 1;

            float ran = Random.Range(0f, 100f);
            foreach (ItemDrop item in itemDrops){
                if (ran < item.chance){
                    var inst = Instantiate(item.item);
                    inst.transform.parent = null;
                    inst.transform.position = transform.position;
                    break;
                }
            }
        }
    }

    public void CheckDeadState(){
        if (currentHealth <= 0) state = STATE.DEAD;
    }

    public void GainHealth(float amount) {
        currentHealth += amount;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        
        healthBar.SetHealth(currentHealth);
    }

    public void SetHealth(float value) {
        currentHealth = value;
        healthBar.SetMaxHealth(value);
    }
}
