using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: https://www.youtube.com/playlist?list=PLJWSdH2kAe_Ij7d7ZFR2NIW8QCJE74CyT
[CreateAssetMenu(fileName = "NewInventory", menuName = "InventorySystem/Inventory")]
public class InventoryObject : ScriptableObject
{
    public Inventory container;
    public List<ItemObject> startItems = new List<ItemObject>();

    public void Init() {
        for (int i = 0; i < startItems.Count; i++) {
            AddItem(startItems[i], 1);
        }
    }

    public void AddItem(ItemObject item, int amount) {
        bool hasItem = false;
        for (int i = 0; i < container.items.Count; i++) {
            if (container.items[i].item.name == item.name) {
                container.items[i].AddAmount(amount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem) {
            InventorySlot slot = new InventorySlot(item, amount);
            container.items.Add(slot);

            HotBar hotBar = GameObject.FindObjectOfType<HotBar>();
            hotBar.AutoAssign(slot);
        }
    }

    public void Remove(InventorySlot slot) {
        container.items.Remove(slot);
        GameObject.FindObjectOfType<DisplayInventory>()?.Remove(slot);
    }
}

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> items = new List<InventorySlot>();
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject item, int amount) {
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }
}