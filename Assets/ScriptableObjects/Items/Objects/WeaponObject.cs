using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponObject", menuName = "InventorySystem/Items/Weapon")]
public class WeaponObject : ItemObject
{
    public int enemyDamagePerHit;
    public int materialDamagePerHit;
    public int range;

    private void Awake() {
        type = ItemType.Weapon;
    }
}
