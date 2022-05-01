using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotBarButton : MonoBehaviour
{
    [SerializeField] private GameObject image;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private TMP_Text slotText;

    public InventoryObject inventory;

    public event Action<int> OnButtonClicked;

    private KeyCode keyCode;
    private int keyNumber;

    private void Awake() {
        keyNumber = transform.GetSiblingIndex() + 1;
        keyCode = KeyCode.Alpha0 + keyNumber;

        image.SetActive(false);
        amountText.gameObject.SetActive(false);

        slotText.SetText(keyNumber.ToString());
        gameObject.name = "HotBar Button " + keyNumber;
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void Update() {
        UpdateDisplay();
        if (Input.GetKeyDown(keyCode)) { HandleClick(); }
    }

    private void UpdateDisplay() {
        InventorySlot slot = inventory.container.items[keyNumber - 1];

        if (slot?.item != null) {
            image.GetComponent<Image>().sprite = slot.item.uiDisplay;
            amountText.text = slot.amount == 1 ? "" : slot.amount.ToString("n0");
        } else {
            image.GetComponent<Image>().sprite = null;
            amountText.text = "";
        }
        
        image.SetActive(slot?.item != null);
        amountText.gameObject.SetActive(slot?.item != null);
        slotText.gameObject.SetActive(slot?.item == null);
    }

    private void HandleClick() {
        if (IsAssigned()) {
            OnButtonClicked?.Invoke(keyNumber - 1);
            SFXManager.instance.Stop("Beam");
        }
    }

    private bool IsAssigned() {
        InventorySlot slot = inventory.container.items[keyNumber - 1];
        return slot.item != null;
    }
}
