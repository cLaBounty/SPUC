using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrill : MonoBehaviour
{
    public float maxHealth = 500f;
    public float currentHealth = 500f;

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

    // Supply Drop Distance Reference
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 75f);
    }
}
