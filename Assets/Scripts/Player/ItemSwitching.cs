using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitching : MonoBehaviour
{
    public void SwitchToItem(ItemObject item) {
        if (transform.childCount > 0) {
            Destroy(transform.GetChild(0).gameObject);
        }
        var obj = Instantiate(item.usablePrefab, transform, false);
        obj.GetComponent<UsableItem>().Init();
    }
}
