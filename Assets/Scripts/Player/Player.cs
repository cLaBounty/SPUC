using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar;
    public HotBar hotBar;
    
    public InventoryObject inventory;
    public CraftingObject crafting;
    private CameraSystem cameraSystem;

    const float ITEM_DROP_DISTANCE = 5f;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar?.SetMaxHealth(maxHealth);
        cameraSystem = GameObject.FindObjectOfType<CameraSystem>();
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
            GroundItem gi = other.GetComponent<GroundItem>();
            SFXManager.instance?.Play(gi.sfx, 0.9f, 1.1f);
            PickUpItem(gi);
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "EnemyProjectile") {
            TakeDamage(other.GetComponent<EnemyProjectile>().damage);
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
