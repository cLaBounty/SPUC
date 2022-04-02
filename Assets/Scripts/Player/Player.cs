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

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            if (InventoryCanvas.InventoryIsOpen || PauseMenu.GameIsPaused) return;
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
        GroundItem groundItem = other.GetComponent<GroundItem>();
        if (groundItem != null) {
            PickUpItem(groundItem);
            Destroy(other.gameObject);
        }
    }

    // HotBar
    public void UseItem() {
        ItemObject item = ItemSelector.GetItem();
        UsableItem usable = ItemSelector.GetUsableItem();
        if (usable != null) {
            usable.Use();
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
