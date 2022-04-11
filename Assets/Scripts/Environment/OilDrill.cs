using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrill : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar billboardHealthBar;
    public HealthBar hudHealthBar;

    void Start()
    {
        currentHealth = maxHealth;
        billboardHealthBar.SetMaxHealth(maxHealth);
        hudHealthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0) {
            currentHealth = 0;
        }

        billboardHealthBar.SetHealth(currentHealth);
        hudHealthBar.SetHealth(currentHealth);
    }

    public void GainHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        
        billboardHealthBar.SetHealth(currentHealth);
        hudHealthBar.SetHealth(currentHealth);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "EnemyProjectile") {
            TakeDamage(other.GetComponent<EnemyProjectile>().damage);
        }
    }
}
