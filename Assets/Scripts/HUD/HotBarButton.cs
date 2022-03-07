using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotBarButton : MonoBehaviour
{
    [SerializeField] private GameObject image;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private TMP_Text slotText;

    public event Action<int, InventorySlot> OnButtonClicked;

    private InventorySlot slot;

    private KeyCode keyCode;
    private int keyNumber;

    public InventorySlot GetSlot() {
        return slot;
    }

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
        UpdateAmount();

        if (Input.GetKeyDown(keyCode) && slot != null) {
            HandleClick();
        }
    }

    private void UpdateAmount() {
        amountText.text = slot?.amount == 1 ? "" : slot?.amount.ToString("n0");
    }

    private void HandleClick() {
        OnButtonClicked?.Invoke(keyNumber - 1, slot);
    }

    public void Assign(InventorySlot slot) {
        this.slot = slot;
        image.GetComponent<Image>().sprite = slot.item.uiDisplay;
        UpdateAmount();
        
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
