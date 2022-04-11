using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour
{
    public static bool InventoryIsOpen = false;

    [SerializeField] private GameObject InventoryCanvasUI;
    [SerializeField] private DisplayInventory InventoryDisplay;
    [SerializeField] private DisplayCrafting CraftingDisplay;
    [SerializeField] private GameObject Scope;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C) && Scope.activeInHierarchy == false) {
            Toggle();
        }
    }

    private void Toggle() {
        InventoryIsOpen = !InventoryIsOpen;

        // Remove info popups and drag sprites
        if (!InventoryIsOpen) {
            InventoryDisplay.CleanUp();
            CraftingDisplay.CleanUp();
        }

        InventoryCanvasUI.SetActive(InventoryIsOpen);
        Cursor.visible = InventoryIsOpen;
        Cursor.lockState = InventoryIsOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}