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
        if (PauseMenu.GameIsPaused) return;

        if (InventoryIsOpen) {
            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyUp(KeyCode.Escape)) {
                Close();
            }
        } else {
            if (Input.GetKeyDown(KeyCode.C) && Scope.activeInHierarchy == false) {
                Open();
            }
        }
    }

    private void Open() {
        InventoryIsOpen = true;
        InventoryCanvasUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Close() {
        InventoryIsOpen = false;

        // Remove Info Popups Drag Sprites
        InventoryDisplay.CleanUp();
        CraftingDisplay.CleanUp();
        
        InventoryCanvasUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}