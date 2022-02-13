using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterialObject", menuName = "InventorySystem/Items/Material")]
public class MaterialObject : ItemObject
{
    private void Awake() {
        //prefab = GameObject.Find("Blue Item Slot");
        type = ItemType.Material;
    }
}
