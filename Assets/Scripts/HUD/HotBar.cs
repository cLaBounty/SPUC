using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: https://www.youtube.com/watch?v=kdckcSwPkrg
public class HotBar : MonoBehaviour
{
    private int SLOTS = 7;
    public InventoryObject inventory;

    private int activeIndex;

    private void Awake() {
        foreach(var button in GetComponentsInChildren<HotBarButton>()) {
            button.OnButtonClicked += ButtonOnButtonClicked;
        }
    }

    private void Start() {
        ButtonOnButtonClicked(0); // Select Resource Beam
    }

    private void ButtonOnButtonClicked(int index) {
        if (InventoryScreenStatus.isOpen) return; // Can't switch items when inventory screen is open
        activeIndex = index;
        ItemSelector.SetItem(inventory.container.items[index].item);
    }

    public void HandleItemUse() {
        InventorySlot activeSlot = inventory.container.items[activeIndex];

        if (activeSlot.amount <= 1) {
            inventory.Remove(activeSlot);
            SelectNewSlot();
        } else {
            activeSlot.amount = activeSlot.amount - 1;
        }
    }

    // Find closest filled slot
    private void SelectNewSlot() {
        for (int i = 1; i < SLOTS; i++) {
            int low = activeIndex - i;
            int high = activeIndex + i;
            if (low >= 0) {
                if (inventory.container.items[low].item != null) {
                    ButtonOnButtonClicked(low);
                    return;
                }
            }

            if (high < SLOTS) {
                if (inventory.container.items[high].item != null) {
                    ButtonOnButtonClicked(high);
                    return;
                }
            }
        }
    }
}

public static class ItemSelector
{
    private static UsableItem Item;

    public static void SetItem(ItemObject item) {
        Item = item.usablePrefab.gameObject.GetComponent<UsableItem>();
        GameObject.FindObjectOfType<ItemSwitching>().SwitchToItem(item);
    }

    public static UsableItem GetItem() {
        return Item;
    }
}