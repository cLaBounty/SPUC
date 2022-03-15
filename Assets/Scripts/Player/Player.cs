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
        inventory.Init();
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
    public void DropItem(InventorySlot slot) {
        for (int i = 0; i < slot.amount; i++) {
            var inst = Instantiate(slot.item.groundPrefab);
            inst.transform.parent = null;
            
            Vector3 cameraDirection = cameraSystem.getMainCamera().transform.forward;
            cameraDirection.y = 0; // Fixes issue of items dropping underground
            inst.transform.position = transform.position + (ITEM_DROP_DISTANCE * cameraDirection);
        }
    }

    public void OnTriggerEnter(Collider other) {
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem != null) {
            inventory.AddItem(groundItem.item, 1);
            Destroy(other.gameObject);
        }
    }

    // HotBar
    public void UseItem() {
        if (InventoryScreenStatus.isOpen) return; // Can't use item when inventory screen is open
        UsableItem usable = ItemSelector.GetItem();
        usable.Use();
        if (usable.item.type == ItemType.Weapon) return; // ToDo: reduce ammo instead
        hotBar.HandleItemUse();
    }

    private void OnApplicationQuit() {
        inventory.container.items = new InventorySlot[28];
        crafting.container.items = new InventorySlot[28];
    }
}
