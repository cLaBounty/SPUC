using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    public HotBar hotBar;
    
    public InventoryObject inventory;

    void Start()
    {
        healthBar = GameObject.FindObjectOfType<HealthBar>();
        hotBar = GameObject.FindObjectOfType<HotBar>();

        currentHealth = maxHealth;
        healthBar?.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            UseItem();
        }

        // TESTING HEALTH BAR
        int amountPerClick = 10;
        if (Input.GetKeyDown(KeyCode.L)) {
            TakeDamage(amountPerClick);
        }
        else if (Input.GetKeyDown(KeyCode.M)) {
            GainHealth(amountPerClick);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0) {
            currentHealth = 0;
        }

        healthBar?.SetHealth(currentHealth);
    }

    void GainHealth(int amount)
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
            inventory.AddItem(new Item(groundItem.item), 1);
            Destroy(other.gameObject);
            Debug.Log($"{groundItem.item.name} collected!");
        }
    }

    private void OnApplicationQuit() {
        inventory.container.items.Clear();
    }

    // HotBar
    public void UseItem() {
        if (SlotSelector.SelectedSlot == null) return; // TODO: remove. should be resource beam by default

        InventorySlot slot = SlotSelector.SelectedSlot;
        ItemObject item = slot.item.itemObject;

        switch(item.type) {
            case ItemType.Barricade:
                PlaceBarricade(item as BarricadeObject);
                break;
            case ItemType.Consumable:
                UseConsumable(item as ConsumableObject);
                break;
            case ItemType.Deployable:
                PlaceDeployable(item as DeployableObject);
                break;
            case ItemType.Weapon:
                UseWeapon(item as WeaponObject);
                break;
            case ItemType.Material:
                Debug.Log("Can't use a material");
                return;
        }

        if (slot.amount <= 1) {
            // Remove from inventory
            //inventory.container.items.Remove(slot);
            
            // Reset hot bar button
            hotBar.ResetSelectedButton();
        } else {
            slot.amount = slot.amount - 1;
        }
    }

    public void PlaceBarricade(BarricadeObject item) {
        Debug.Log($"{item.name} placed!");
    }

    public void UseConsumable(ConsumableObject item) {
        Debug.Log($"{item.name} consumed!");
        GainHealth(item.healthIncreaseValue);
    }

    public void PlaceDeployable(DeployableObject item) {
        Debug.Log($"{item.name} deployed!");
    }

    public void UseWeapon(WeaponObject item) {
        Debug.Log($"{item.name} used!");
    }
}
