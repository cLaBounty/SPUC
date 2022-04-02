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
        inventory.Init();
        SelectSlot(0); // Select Resource Beam
    }

    private void ButtonOnButtonClicked(int index) {
        if (InventoryCanvas.InventoryIsOpen || PauseMenu.GameIsPaused) return;
        SelectSlot(index);
    }

    private void SelectSlot(int index) {
        activeIndex = index;
        ItemSelector.SetItem(inventory.container.items[index].item);
    }

    public void HandleItemUse(ItemObject item) {
        int index = inventory.GetIndex(item);
        InventorySlot slot = inventory.container.items[index];

        if (slot.amount <= 1) {
            inventory.Remove(slot);
            if (index == activeIndex) { SelectNewSlot(); }
        } else {
            slot.amount = slot.amount - 1;
        }
    }

    // Find closest filled slot
    private void SelectNewSlot() {
        for (int i = 1; i < SLOTS; i++) {
            int low = activeIndex - i;
            int high = activeIndex + i;
            if (low >= 0) {
                if (inventory.container.items[low].item != null) {
                    SelectSlot(low);
                    return;
                }
            }

            if (high < SLOTS) {
                if (inventory.container.items[high].item != null) {
                    SelectSlot(high);
                    return;
                }
            }
        }
    }

    public void NotifySwap(ItemObject item1, ItemObject item2) {
        bool isItem1 = (item1 == ItemSelector.GetItem());
        bool isItem2 = (item2 == ItemSelector.GetItem());
        if (isItem1 || isItem2) {
            int newIndex = inventory.GetIndex(ItemSelector.GetItem());
            if (newIndex < SLOTS) {
                activeIndex = newIndex;
            } else {
                if ((isItem1 && item2 != null) || (isItem2 && item1 != null)) { activeIndex++; }
                SelectNewSlot();
            }
        }
    }

    public void NotifyDrop(ItemObject item) {
        if (item == ItemSelector.GetItem()) {
            SelectNewSlot();
        }
    }
}

public static class ItemSelector
{
    private static ItemObject currentItem;

    public static void SetItem(ItemObject item) {
        currentItem = item;
        GameObject.FindObjectOfType<ItemSwitching>().SwitchToItem(item);
    }

    public static ItemObject GetItem() {
        return currentItem;
    }

    public static UsableItem GetUsableItem() {
        return currentItem.holdPrefab.gameObject.GetComponent<UsableItem>();
    }
}