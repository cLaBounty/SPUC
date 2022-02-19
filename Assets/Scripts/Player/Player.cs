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

    private void Start()
    {
        healthBar = GameObject.FindObjectOfType<HealthBar>();
        hotBar = GameObject.FindObjectOfType<HotBar>();

        currentHealth = maxHealth;
        healthBar?.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            UseItem();
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

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
    public void OnTriggerEnter(Collider other) {
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem != null) {
            inventory.AddItem(groundItem.item, 1);
            Destroy(other.gameObject);
            Debug.Log($"{groundItem.item.name} collected!");
        }
    }

    private void OnApplicationQuit() {
        inventory.container.items.Clear();
    }

    // HotBar
    public void UseItem() {
        UsableItem usable = ItemSelector.GetItem();
        if (usable == null) return; // Can't be used
        usable.Use();
        if (usable.item.type == ItemType.Weapon) return; // ToDo: reduce ammo instead
        hotBar.HandleItemUse();
    }
}
