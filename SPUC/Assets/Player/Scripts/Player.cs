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
        // TESTING
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
        var item = other.GetComponent<Item>();
        if (item != null) {
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
            Debug.Log($"{item.item.name} collected!");
        }
    }

    private void OnApplicationQuit() {
        inventory.Container.Clear();
    }
}
