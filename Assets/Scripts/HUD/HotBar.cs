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
        inventory.Init();

        // Select Resource Beam
        ButtonOnButtonClicked(0, transform.GetChild(0).GetComponent<HotBarButton>().GetSlot());
    }

    public void AutoAssign(InventorySlot slot) {
        for (int i = 0; i < SLOTS; i++) {
            HotBarButton button = transform.GetChild(i).GetComponent<HotBarButton>();

            if (button.GetSlot() == null) {
                button.Assign(slot);
                return;
            }
        }
    }

    private void ButtonOnButtonClicked(int index, InventorySlot slot) {
        SlotSelector.SelectedIndex = index;
        SlotSelector.SelectedSlot = slot;
        Debug.Log($"{slot.item.name} is active!");
    }

    public void ResetButton(int index) {
        transform.GetChild(index).GetComponent<HotBarButton>().Reset();
        SelectNewSlot(index);
    }

    // Find closest filled slot
    private void SelectNewSlot(int prevIndex) {
        for (int i = 1; i < SLOTS; i++) {
            int low = prevIndex - i;
            int high = prevIndex + i;
            if (low >= 0) {
                InventorySlot slot = transform.GetChild(low).GetComponent<HotBarButton>().GetSlot();
                if (slot != null) {
                    ButtonOnButtonClicked(low, slot);
                    return;
                }
            }

            if (high < SLOTS) {
                InventorySlot slot = transform.GetChild(high).GetComponent<HotBarButton>().GetSlot();
                if (slot != null) {
                    ButtonOnButtonClicked(high, slot);
                    return;
                }
            }
        }
    }
}

public static class SlotSelector
{
    public static int SelectedIndex { get; set; }
    public static InventorySlot SelectedSlot { get; set; }
}