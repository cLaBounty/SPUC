using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponObject", menuName = "InventorySystem/Items/Weapon")]
public class WeaponObject : ItemObject
{
    private void Awake() {
        type = ItemType.Weapon;
    }
}
