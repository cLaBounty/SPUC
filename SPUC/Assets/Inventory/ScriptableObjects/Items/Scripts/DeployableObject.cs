using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDeployableObject", menuName = "InventorySystem/Items/Deployable")]
public class DeployableObject : ItemObject
{
    private void Awake() {
        type = ItemType.Deployable;
    }
}