using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftingSystem", menuName = "CraftingSystem/Crafting")]
public class CraftingObject : ScriptableObject
{
    public bool displayAvailableOnly = false;
    
    public InventoryObject playerInventory;
    public Inventory container;

    public void Update() {
        foreach (InventorySlot slot in container.items) {
            slot.Update(null, 0);
        }

        ItemObject[] allItems = Resources.LoadAll<ItemObject>("Items");
        foreach (ItemObject item in allItems) {
            if (displayAvailableOnly) { // Available
                if (IsCraftable(item)) { AddItem(item); }
            } else { // All Items
                if (item.recipe.Length > 0) { AddItem(item); }
            }
        }
    }

    public bool IsCraftable(ItemObject item) {
        if (item.recipe.Length <= 0) return false; // can't craft item without recipe
        foreach (Ingredient ingredient in item.recipe) {
            if (!playerInventory.Has(ingredient.item, ingredient.amount)) {
                return false;
            }
        }
        return true;
    }

    private void AddItem(ItemObject item) {
        InventorySlot slot = GetFirstEmptySlot();
        slot.Update(item, 1);
    }

    public void CraftItem(InventorySlot slot) {
        if (!IsCraftable(slot.item)) return;
        ItemObject newItem = slot.item;
        playerInventory.RemoveItems(slot.item.recipe);
        playerInventory.AddItem(newItem, newItem.craftAmount);
        slot.Update(null, 0);
        Update();
    }

    private InventorySlot GetFirstEmptySlot() {
        for (int i = 0; i < container.items.Length; i++) {
            if (container.items[i].item == null) {
                return container.items[i];
            }
        }
        return null; // Crafting Menu is full
    }

    public int GetIndex(ItemObject item) {
        for (int i = 0; i < container.items.Length; i++) {
            if (container.items[i].item == item) {
                return i;
            }
        }
        return -1; // not found
    }
}