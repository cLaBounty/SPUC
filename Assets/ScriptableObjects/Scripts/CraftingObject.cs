using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftingSystem", menuName = "CraftingSystem/Crafting")]
public class CraftingObject : ScriptableObject
{
    public InventoryObject playerInventory;
    public Inventory container;

    public void Update() {
        foreach (InventorySlot slot in container.items) {
            slot.Update(null, 0);
        }

        ItemObject[] allItems = Resources.LoadAll<ItemObject>("Items");
        foreach (ItemObject item in allItems) {
            bool shouldAdd = true;
            foreach (Ingredient ingredient in item.recipe) {
                if (!playerInventory.Has(ingredient.item, ingredient.amount)) {
                    shouldAdd = false;
                    break;
                }
            }
            if (shouldAdd && item.recipe.Length > 0) { AddItem(item); }
        }
    }

    private void AddItem(ItemObject item) {
        InventorySlot slot = GetFirstEmptySlot();
        slot.Update(item, 1);
    }

    public void CraftItem(InventorySlot slot) {
        ItemObject newItem = slot.item;
        playerInventory.RemoveItems(slot.item.recipe);
        playerInventory.AddItem(newItem, 1);
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
}