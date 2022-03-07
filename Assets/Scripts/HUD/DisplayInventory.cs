using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField] private float X_START;
    [SerializeField] private float Y_START;
    [SerializeField] private int X_SPACE_BETWEEN_ITEMS;
    [SerializeField] private int Y_SPACE_BETWEEN_ITEMS;
    [SerializeField] private int NUMBER_OF_COLUMNS;
    
    public GameObject inventoryPrefab;
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> inventoryItems = new Dictionary<GameObject, InventorySlot>();

    private MouseItem mouseItem = new MouseItem();

    private void Start()
    {
        CreateSlots();
    }

    private void Update()
    {
        UpdateSlots();
    }

    private void CreateSlots() {
        for (int i = 0; i < inventory.container.items.Length; i++) {
            GameObject obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            inventoryItems.Add(obj, inventory.container.items[i]);
        }
    }

    private void UpdateSlots() {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in inventoryItems) {
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

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj) {
        mouseItem.hoverObj = obj;
        if (inventoryItems.ContainsKey(obj)) {
            mouseItem.hoverItem = inventoryItems[obj];
        }
    }

    public void OnExit(GameObject obj) {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj) {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(35, 35);
        mouseObject.transform.SetParent(transform.parent);

        if (inventoryItems[obj].item != null) {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventoryItems[obj].item.uiDisplay;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = inventoryItems[obj];
    }

    public void OnDragEnd(GameObject obj) {
        InventorySlot slot = inventoryItems[obj];
        if (mouseItem.hoverObj != null) {
            inventory.SwapItems(slot, inventoryItems[mouseItem.hoverObj]);
        } else {
            if (slot.item.isDroppable) {
                // ToDo: fix
                // var inst = Instantiate(slot.item.groundPrefab);
                // inst.transform.parent = null;
                // inst.transform.position = GameObject.FindObjectOfType<Player>().transform.position + new Vector3(5, 0, 0);

                inventory.Remove(slot);
            }
        }

        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }

    public void OnDrag(GameObject obj) {
        if (mouseItem.obj != null) {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    private Vector3 GetPosition(int i) {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS)), 0f);
    }
}

public class MouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}