using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField] private int X_START;
    [SerializeField] private int Y_START;
    [SerializeField] private int X_SPACE_BETWEEN_ITEMS;
    [SerializeField] private int Y_SPACE_BETWEEN_ITEMS;
    [SerializeField] private int NUMBER_OF_COLUMNS;

    public GameObject inventoryPrefab;
    public InventoryObject inventory;
    public Dictionary<InventorySlot, GameObject> inventoryItems = new Dictionary<InventorySlot, GameObject>();

    void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay() {
        for (int i = 0; i < inventory.container.items.Count; i++) {
            InventorySlot slot = inventory.container.items[i];

            if (inventoryItems.ContainsKey(slot)) {
                inventoryItems[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            }
            else {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.item.uiDisplay;
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                inventoryItems.Add(slot, obj);
            }
        }
    }

    public void UpdatePositions() {
        for (int i = 0; i < inventory.container.items.Count; i++) {
            InventorySlot slot = inventory.container.items[i];
            inventoryItems[slot].GetComponent<RectTransform>().localPosition = GetPosition(i);
        }
    }

    public void Remove(InventorySlot slot) {
        Destroy(inventoryItems[slot]);
        inventoryItems.Remove(slot);
        UpdatePositions();
    }

    private Vector3 GetPosition(int i) {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS)), 0f);
    }
}
