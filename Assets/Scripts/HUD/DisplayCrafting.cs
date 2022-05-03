using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class DisplayCrafting : MonoBehaviour
{
    [SerializeField] private float X_START;
    [SerializeField] private float Y_START;
    [SerializeField] private int X_SPACE_BETWEEN_ITEMS;
    [SerializeField] private int Y_SPACE_BETWEEN_ITEMS;
    [SerializeField] private int NUMBER_OF_COLUMNS;
    
    public GameObject itemPrefab;
    public GameObject itemInfoPrefab;
    private GameObject currentItemInfo = null;

    public CraftingObject crafting;
    public Dictionary<GameObject, InventorySlot> craftingItems = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        CreateSlots();
    }

    private void Update()
    {
        UpdateSlots();
    }

    private void CreateSlots() {
        for (int i = 0; i < crafting.container.items.Length; i++) {
            GameObject obj = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { /* Called from button OnPointerClick */ });

            craftingItems.Add(obj, crafting.container.items[i]);
        }
    }

    private void UpdateSlots() {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in craftingItems) {
            if (slot.Value.item != null) {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.item.uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.item.craftAmount == 1 ? "" : slot.Value.item.craftAmount.ToString("n0");
                slot.Key.transform.GetChild(2).gameObject.SetActive(!crafting.IsCraftable(slot.Value.item)); // overlay if not craftable
            } else {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                slot.Key.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    public void SetDisplayAvailableOnly(bool value) {
        crafting.displayAvailableOnly = value;
        crafting.Update();
    }

    private Vector3 GetPosition(int i) {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS)), 0f);
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj) {
        if (craftingItems[obj].item == null) return; // Empty slot
        CleanUp();
        
        float ratioX = 210f / 1920f;
        float ratioY = -85f / 1080f;
        Vector3 offset = new Vector3(ratioX * Screen.width, ratioY * Screen.height, 0);

        if (crafting.GetIndex(craftingItems[obj].item) % NUMBER_OF_COLUMNS > 4) { offset.x *= -1; }
        currentItemInfo = Instantiate(itemInfoPrefab, obj.transform.position + offset, Quaternion.identity, transform);
        currentItemInfo.GetComponent<DisplayCraftingItemInfo>().SetUp(craftingItems[obj].item, crafting.playerInventory);
    }

    public void OnExit(GameObject obj) {
        if (craftingItems[obj].item == null) return; // Empty slot
        CleanUp();
    }

    public void OnClick(GameObject obj, PointerEventData data) {
        if (craftingItems[obj].item == null) return; // Empty slot
        CleanUp();

        if (crafting.IsCraftable(craftingItems[obj].item)) SFXManager.instance.Play("Craft", 0.9f, 1.1f);

        switch(data.button) {
            case PointerEventData.InputButton.Left:
                CraftOnce(obj);
                break;
            case PointerEventData.InputButton.Right:
                CraftFiveTimes(obj);
                break;
            case PointerEventData.InputButton.Middle:
                CraftAll(obj);
                break;
        }

        OnEnter(obj); // re-show info if item is still there
    }

    private void CraftOnce(GameObject obj) {
        crafting.CraftItem(craftingItems[obj]);
    }

    private void CraftFiveTimes(GameObject obj) {
        ItemObject item = craftingItems[obj].item;
        int i = 0;
        while(i < 5 && crafting.IsCraftable(item)) {
            crafting.CraftItem(craftingItems[obj]);
            i++;
        }
    }

    private void CraftAll(GameObject obj) {
        ItemObject item = craftingItems[obj].item;
        while(crafting.IsCraftable(item)) {
            crafting.CraftItem(craftingItems[obj]);
        }
    }

    public void CleanUp() {
        if (currentItemInfo != null) {
            Destroy(currentItemInfo.gameObject);
        }
    }
}