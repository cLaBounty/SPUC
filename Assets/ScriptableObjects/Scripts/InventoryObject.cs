using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: https://www.youtube.com/playlist?list=PLJWSdH2kAe_Ij7d7ZFR2NIW8QCJE74CyT
[CreateAssetMenu(fileName = "NewInventory", menuName = "InventorySystem/Inventory")]
public class InventoryObject : ScriptableObject
{
    public Inventory container;
    public List<ItemObject> startItems;
    public CraftingObject crafting;

    private HotBar hotBar;

    public void Init() {
        hotBar = GameObject.FindObjectOfType<HotBar>();
        for (int i = 0; i < startItems.Count; i++) {
            AddItem(startItems[i], 1);
        }
    }

    public void AddItem(ItemObject item, int amount) {
        bool hasItem = false;
        for (int i = 0; i < container.items.Length; i++) {
            if (container.items[i].item?.name == item.name) {
                container.items[i].AddAmount(amount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem) {
            InventorySlot slot = GetFirstEmptySlot();
            slot.Update(item, amount);
        }

        crafting.Update();
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2) {
        InventorySlot tmp = new InventorySlot(item2.item, item2.amount);
        item2.Update(item1.item, item1.amount);
        item1.Update(tmp.item, tmp.amount);
    }

    private InventorySlot GetFirstEmptySlot() {
        for (int i = 0; i < container.items.Length; i++) {
            if (container.items[i].item == null) {
                return container.items[i];
            }
        }
        return null; // Inventory is full
    }

    public void Remove(InventorySlot slot) {
        slot.Update(null, 0);
        crafting.Update();
    }

    public void Reduce(InventorySlot slot, int amount) {
        slot.ReduceAmount(amount);
        crafting.Update();
    }

    public void RemoveItems(Ingredient[] recipe) {
        foreach (Ingredient ingredient in recipe) {
            foreach (InventorySlot slot in container.items) {
                if (slot.item == ingredient.item) {
                    if (slot.amount > ingredient.amount) {
                        Reduce(slot, ingredient.amount);
                    } else {
                        Remove(slot);
                        if (GetIndex(slot.item) == hotBar.activeIndex) { hotBar.SelectNewSlot(); }
                    }
                    break;
                }
            }
        }
    }

    public bool Has(ItemObject item, int amount) {
        foreach (InventorySlot slot in container.items) {
            if (slot?.item == item && slot?.amount >= amount) {
                return true;
            }
        }
        return false;
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

[System.Serializable]
public class Inventory
{
    public InventorySlot[] items = new InventorySlot[28];
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot() {
        this.item = null;
        this.amount = 0;
    }

    public InventorySlot(ItemObject item, int amount) {
        this.item = item;
        this.amount = amount;
    }

    public void Update(ItemObject item, int amount) {
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }

    public void ReduceAmount(int value) {
        amount -= value;
    }
}