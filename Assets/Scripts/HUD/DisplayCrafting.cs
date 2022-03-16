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
    
    public GameObject craftingPrefab;
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
            GameObject obj = Instantiate(craftingPrefab, Vector3.zero, Quaternion.identity, transform);
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
            } else {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
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
        CleanUp();
        currentItemInfo = Instantiate(itemInfoPrefab, obj.transform.position + new Vector3(105, -17, 0), Quaternion.identity, transform);
        currentItemInfo.GetComponent<DisplayCraftingItemInfo>().SetUp(craftingItems[obj].item);
    }

    public void OnExit(GameObject obj) {
        if (craftingItems[obj].item == null) return; // Empty slot
        CleanUp();
    }

    public void OnClick(GameObject obj) {
        if (craftingItems[obj].item == null) return; // Empty slot
        CleanUp();
        crafting.CraftItem(craftingItems[obj]);
    }

    public void CleanUp() {
        if (currentItemInfo != null) {
            Destroy(currentItemInfo.gameObject);
        }
    }
}