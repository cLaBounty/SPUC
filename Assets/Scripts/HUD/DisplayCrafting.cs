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
    
    public GameObject inventoryPrefab;
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
            GameObject obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });

            craftingItems.Add(obj, crafting.container.items[i]);
        }
    }

    private void UpdateSlots() {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in craftingItems) {
            if (slot.Value.item != null) {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.item.uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
            } else {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
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
        // ToDo: show stats and recipe
    }

    public void OnExit(GameObject obj) {
        if (craftingItems[obj].item == null) return; // Empty slot
        // ToDo: hide stats and recipe
    }

    public void OnClick(GameObject obj) {
        if (craftingItems[obj].item == null) return; // Empty slot
        crafting.CraftItem(craftingItems[obj]);
    }
}