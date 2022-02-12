using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public InventoryObject inventory;

    void Start()
    {
        if (healthBar == null) {
            healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        }
        currentHealth = maxHealth;
        healthBar?.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        // TESTING HEALTH BAR
        int amountPerClick = 10;
        if (Input.GetKeyDown(KeyCode.L)) {
            if (currentHealth >= amountPerClick) {
                TakeDamage(amountPerClick);
            }
        }
        else if (Input.GetKeyDown(KeyCode.M)) {
            if (currentHealth <= maxHealth - amountPerClick) {
                GainHealth(amountPerClick);
            }
        }
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthBar?.SetHealth(currentHealth);
    }

    void GainHealth(int amount)
    {
        currentHealth += amount;
        healthBar?.SetHealth(currentHealth);
    }

    // Inventory
    public void OnTriggerEnter(Collider other) {
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem != null) {
            inventory.AddItem(new Item(groundItem.item), 1);
            Destroy(other.gameObject);
            Debug.Log($"{groundItem.item.name} collected!");
        }
    }

    private void OnApplicationQuit() {
        inventory.container.items.Clear();
    }
}
