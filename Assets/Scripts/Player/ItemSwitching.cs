using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSwitching : MonoBehaviour
{
    public void SwitchToItem(ItemObject item) {
        if (transform.childCount > 0) {
            Destroy(transform.GetChild(0).gameObject);
        }
        var inst = Instantiate(item.holdPrefab, transform, false);
        if (inst.transform.GetComponent<UsableItem>() == null) {
            GameObject.FindGameObjectWithTag("Crosshair").transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
    }
}
