using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    [SerializeField] float itemGrav = 1f;
    [SerializeField] float itemRange = 2f;

    public HealthBar healthBar;
    public HotBar hotBar;
    
    public InventoryObject inventory;
    public CraftingObject crafting;
    private CameraSystem cameraSystem;

    const float ITEM_DROP_DISTANCE = 5f;

    LayerMask itemMask;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar?.SetMaxHealth(maxHealth);
        cameraSystem = GameObject.FindObjectOfType<CameraSystem>();
        itemMask = LayerMask.GetMask("Ground Item");
    }

    void Update() {
        //move away from other objects
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, itemRange, itemMask);
        Vector2 totalForce = Vector2.zero;

        foreach(Collider col in hitColliders){
            col.transform.parent.position += (transform.position - col.transform.parent.position).normalized * itemGrav * Time.deltaTime;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        SFXManager.instance?.Play("Hurt");

        if (currentHealth < 0) {
            currentHealth = 0;
        }

        healthBar?.SetHealth(currentHealth);
    }

    public void GainHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        
        healthBar?.SetHealth(currentHealth);
    }

    // Inventory
    public void PickUpItem(GroundItem groundItem) {
        inventory.AddItem(groundItem.item, groundItem.amount);
    }

    public void DropItem(InventorySlot slot) {
        var inst = Instantiate(slot.item.groundPrefab);
        inst.GetComponent<GroundItem>().amount = slot.amount;
        
        Vector3 dropPosition = transform.position + (ITEM_DROP_DISTANCE * cameraSystem.getMainCamera().transform.forward);
        dropPosition.y = 0.5f; // Fixes issue of items dropping underground
        inst.transform.position = dropPosition;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "GroundItem") {
            GroundItem gi = other.gameObject.GetComponent<GroundItem>();
            SFXManager.instance?.Play(gi.sfx, 0.9f, 1.1f);
            PickUpItem(gi);
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "EnemyProjectile") {
            TakeDamage(other.gameObject.GetComponent<EnemyProjectile>().damage);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "GroundItem") {
            GroundItem gi = other.gameObject.GetComponent<GroundItem>();
            SFXManager.instance?.Play(gi.sfx, 0.9f, 1.1f);
            PickUpItem(gi);
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "EnemyProjectile") {
            TakeDamage(other.gameObject.GetComponent<EnemyProjectile>().damage);
        }
    }

    public void CleanUp() {
        inventory.container.items = new InventorySlot[28];
        crafting.container.items = new InventorySlot[28];
    }

    private void OnApplicationQuit() {
        CleanUp();
    }
}
