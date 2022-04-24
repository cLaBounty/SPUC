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

    //[Header("Debuging")]
    //[SerializeField] 
    [HideInInspector]public GameObject target;
    [HideInInspector]public GridController flowField = null;
    [HideInInspector]public PlayerMovement player = null;
    [HideInInspector]public Player playerStats = null;
    [HideInInspector]public LevelManager levelManager = null;

    public STATE state = STATE.AGRO_OIL;

    public HealthBar healthBar;
    public float currentHealth = 100;

    [Header("Drops (Organize in Order Please)")]
    [SerializeField] ItemDrop[] itemDrops;

    float maxHealth = 0f;
    bool firstSetHealth = false; 

    protected void Start() {
        //SetHealth(currentHealth); // ToDo: remove when health is set in level manager
        healthBar.transform.gameObject.SetActive(false);
    }

    public void TakeDamage (float damage) {
        if (state == STATE.DEAD) return;

        currentHealth -= damage - defense;
        healthBar.transform.gameObject.SetActive(true);
        healthBar.SetHealth(currentHealth);

        SFXManager.instance.Play("Enemy Hurt", 1.4f, 1.7f);

        if (currentHealth <= 0) {
            healthBar.transform.gameObject.SetActive(false);

            // Kill Enemy
            Collider collider = GetComponent<Collider>();
            if (collider != null ) collider.enabled = false;
            state = STATE.DEAD;

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

    public void SetHealth(float value) {
        currentHealth = value;
        healthBar.SetMaxHealth(currentHealth);
    }

    protected void OnDestroy() {
        if (levelManager != null) levelManager.EnemyKilled();
    }
}
