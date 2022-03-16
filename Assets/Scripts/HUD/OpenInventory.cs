using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    [SerializeField] private GameObject InventoryCanvas;
    private DisplayInventory InventoryDisplay;
    private DisplayCrafting CraftingDisplay;

    private void Start() {
        InventoryDisplay = GameObject.FindObjectOfType<DisplayInventory>();
        CraftingDisplay = GameObject.FindObjectOfType<DisplayCrafting>();

        InventoryScreenStatus.isOpen = true;
        Toggle();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Toggle();
        }
    }

    private void Toggle() {
        InventoryScreenStatus.isOpen = !InventoryScreenStatus.isOpen;
        InventoryCanvas?.SetActive(InventoryScreenStatus.isOpen);
        Cursor.visible = InventoryScreenStatus.isOpen;
        Cursor.lockState = InventoryScreenStatus.isOpen ? CursorLockMode.None : CursorLockMode.Locked;

        // Remove info popups and drag sprites
        if (!InventoryScreenStatus.isOpen) {
            InventoryDisplay.CleanUp();
            CraftingDisplay.CleanUp();
        }
    }
}

public static class InventoryScreenStatus {
    public static bool isOpen { set; get; }
}