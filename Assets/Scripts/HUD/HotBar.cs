using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: https://www.youtube.com/watch?v=kdckcSwPkrg
public class HotBar : MonoBehaviour
{
    [SerializeField] private int slots = 7;

    public InventoryObject inventory;
    public int activeIndex;

    private void Awake() {
        foreach(var button in GetComponentsInChildren<HotBarButton>()) {
            button.OnButtonClicked += ButtonOnButtonClicked;
        }
    }

    private void Start() {
        inventory.Init();
        SelectSlot(0); // Select Resource Beam
    }

    private void Update() {
        if (InventoryCanvas.InventoryIsOpen || PauseMenu.GameIsPaused) return;

        // Scroll Wheel Switching
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            for (int i = 1; i < slots; i++) {
                int low = activeIndex - i;
                if (low < 0) { low += slots; }
                if (inventory.container.items[low].item != null) {
                    SelectSlot(low);
                    return;
                }
            }
        } else if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            for (int i = 1; i < slots; i++) {
                int high = (activeIndex + i) % slots;
                if (inventory.container.items[high].item != null) {
                    SelectSlot(high);
                    return;
                }
            }
        }
    }

    private void ButtonOnButtonClicked(int index) {
        if (InventoryCanvas.InventoryIsOpen || PauseMenu.GameIsPaused || activeIndex == index) return;
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
    public void SelectNewSlot() {
        for (int i = 1; i < slots; i++) {
            int low = activeIndex - i;
            int high = activeIndex + i;
            if (low >= 0) {
                if (inventory.container.items[low].item != null) {
                    SelectSlot(low);
                    return;
                }
            }

            if (high < slots) {
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
            if (newIndex < slots) {
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