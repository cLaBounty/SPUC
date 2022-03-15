using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBarricadeObject", menuName = "InventorySystem/Items/Barricade")]
public class BarricadeObject : ItemObject
{
    private void Awake() {
        type = ItemType.Barricade;
    }
}