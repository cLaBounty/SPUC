using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotBarButton : MonoBehaviour
{
    [SerializeField] private GameObject image;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private TMP_Text slotText;
    
    InventorySlot slot = null;

    public event Action<int> OnButtonClicked;

    private KeyCode keyCode;
    private int keyNumber;

    private void OnValidate() {
        keyNumber = transform.GetSiblingIndex() + 1;
        keyCode = KeyCode.Alpha0 + keyNumber;

        image.SetActive(false);
        amountText.gameObject.SetActive(false);

        slotText.SetText(keyNumber.ToString());
        gameObject.name = "HotBar Button " + keyNumber;
    }

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void Update() {
        UpdateDisplay();

        if (Input.GetKeyDown(keyCode)) {
            HandleClick();
        }
    }

    private void UpdateDisplay() {
        if (slot != null) {
            image.GetComponent<Image>().sprite = slot.item.uiDisplay;
            amountText.text = slot.amount.ToString("n0");
        }
    }

    private void HandleClick() {
        OnButtonClicked?.Invoke(keyNumber);
    }

    public void Assign(InventorySlot slot) {
        this.slot = slot;
        UpdateDisplay();

        image.SetActive(true);
        amountText.gameObject.SetActive(true);
        slotText.gameObject.SetActive(false);
    }

    public void Reset() {
        slot = null;

        image.SetActive(false);
        amountText.gameObject.SetActive(false);
        slotText.gameObject.SetActive(true);
    }

    public bool IsAssigned() { return slot != null; }
}
