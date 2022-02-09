using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

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
}
