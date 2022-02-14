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

    private void Start() {
        SlotSelector.SelectedIndex = -1; // TODO: change to resouce beam
        SlotSelector.SelectedSlot = null; // TODO: change to resouce beam
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

    private void ButtonOnButtonClicked(int index, InventorySlot slot) {
        SlotSelector.SelectedIndex = index;
        SlotSelector.SelectedSlot = slot;
        Debug.Log($"{slot.item.name} is active!");
    }

    public void ResetSelectedButton() {
        transform.GetChild(SlotSelector.SelectedIndex).GetComponent<HotBarButton>().Reset();
        SelectNewSlot();
    }

    private void SelectNewSlot() {
        SlotSelector.SelectedIndex = -1;
        SlotSelector.SelectedSlot = null;
    }
}

public static class SlotSelector
{
    public static int SelectedIndex { get; set; }
    public static InventorySlot SelectedSlot { get; set; }
}