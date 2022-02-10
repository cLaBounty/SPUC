using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableObject", menuName = "InventorySystem/Items/Consumable")]
public class ConsumableObject : ItemObject
{
    public int healthIncreaseValue;
    
    private void Awake() {
        type = ItemType.Consumable;
    }
}