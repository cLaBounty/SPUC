using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterialObject", menuName = "InventorySystem/Items/Material")]
public class MaterialObject : ItemObject
{
    private void Awake() {
        type = ItemType.Material;
    }
}
