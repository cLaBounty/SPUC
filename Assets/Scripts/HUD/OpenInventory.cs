using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    public GameObject InventoryCanvas;

    private void Start() {
        InventoryScreenStatus.isOpen = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            Toggle();
        }
    }

    private void Toggle() {
        InventoryScreenStatus.isOpen = !InventoryScreenStatus.isOpen;
        InventoryCanvas?.SetActive(InventoryScreenStatus.isOpen);
        Cursor.visible = InventoryScreenStatus.isOpen;
        Cursor.lockState = InventoryScreenStatus.isOpen ? CursorLockMode.None : CursorLockMode.Locked;      
    }
}

public static class InventoryScreenStatus {
    public static bool isOpen { set; get; }
}