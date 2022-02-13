using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: https://www.youtube.com/playlist?list=PLJWSdH2kAe_Ij7d7ZFR2NIW8QCJE74CyT
[CreateAssetMenu(fileName = "NewInventory", menuName = "InventorySystem/Inventory")]
public class InventoryObject : ScriptableObject
{
    public Inventory container;

    public void AddItem(Item item, int amount) {
        bool hasItem = false;
        for (int i = 0; i < container.items.Count; i++) {
            if (container.items[i].item.id == item.id) {
                container.items[i].AddAmount(amount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem) {
            container.items.Add(new InventorySlot(item.id, item, amount));
        }
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
    public int id;
    public Item item;
    public int amount;

    public InventorySlot(int id, Item item, int amount) {
        this.id = id;
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }
}