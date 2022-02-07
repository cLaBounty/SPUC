using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InventoryScriptableObject", order = 1)]
public class Inventory : ScriptableObject
{
    // Key is the Item object, Value is the count
    Dictionary<Item, int> items = new Dictionary<Item, int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PickUpItem(Item item) {
        int temp;
        if (items.TryGetValue(item, out temp)) { // already has item
            items[item] = items[item] + 1;
        } else {
            items.Add(item, 1);
        }
    }
}
