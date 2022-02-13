using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: https://www.youtube.com/watch?v=kdckcSwPkrg
public class HotBar : MonoBehaviour
{
    private int SLOTS = 7;
    public InventoryObject inventory;

    private void Awake() {
        foreach(var button in GetComponentsInChildren<HotBarButton>()) {
            button.OnButtonClicked += ButtonOnButtonClicked;
        }
    }

    private void Update() {
        // TESTING AUTO HOTBAR FILL
        for (int i = 0; i < SLOTS; i++) {
            HotBarButton button = transform.GetChild(i).GetComponent<HotBarButton>();
            if (!button.IsAssigned() && inventory.container.items.Count > i) {
                InventorySlot slot = inventory.container.items[i];
                button.Assign(slot);
            }
        }
    }

    private void ButtonOnButtonClicked(int buttonNumber) {
        ItemSelector.SelectedItemIndex = buttonNumber - 1;
        Debug.Log($"Button {buttonNumber} clicked!");
    }
}

public static class ItemSelector
{
    public static int SelectedItemIndex { get; set; }
}