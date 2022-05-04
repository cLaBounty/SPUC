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
    
    public GameObject itemPrefab;
    public GameObject itemInfoPrefab;
    private GameObject currentItemInfo = null;
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> inventoryItems = new Dictionary<GameObject, InventorySlot>();

    private Player player;
    private MouseItem mouseItem = new MouseItem();

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        CreateSlots();
    }

    private void Update()
    {
        UpdateSlots();
    }

    private void CreateSlots() {
        for (int i = 0; i < inventory.container.items.Length; i++) {
            GameObject obj = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.transform.GetChild(2).transform.GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            inventoryItems.Add(obj, inventory.container.items[i]);
        }
    }

    private void UpdateSlots() {
        int index = 0;
        foreach (KeyValuePair<GameObject, InventorySlot> slot in inventoryItems) {
            if (slot.Value.item != null) {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.item.uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
                slot.Key.transform.GetChild(2).gameObject.SetActive(false);
            } else {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                if (index < 7) { slot.Key.transform.GetChild(2).gameObject.SetActive(true); }
            }
            index++;
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
        mouseItem.hoverObj = obj;
        if (inventoryItems.ContainsKey(obj)) {
            mouseItem.hoverItem = inventoryItems[obj];

            // Display info, if not dragging an item and not empty
            if (mouseItem.obj == null && mouseItem.item == null && inventoryItems[obj].item != null) {
                float ratioX = 210f / 1920f;
                float ratioY = -42.5f / 1080f;
                Vector3 offset = new Vector3(ratioX * Screen.width, ratioY * Screen.height, 0);

                currentItemInfo = Instantiate(itemInfoPrefab, obj.transform.position + offset, Quaternion.identity, transform);
                currentItemInfo.GetComponent<DisplayInventoryItemInfo>().SetUp(inventoryItems[obj].item);
            }
        }
    }

    public void OnExit(GameObject obj) {
        ClearInfo();
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj) {
        ClearInfo();
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(35, 35);
        mouseObject.transform.SetParent(transform.parent);

        if (inventoryItems[obj].item != null) {
            SFXManager.instance.Play("Move Item", 0.9f, 1.1f);
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventoryItems[obj].item.uiDisplay;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = inventoryItems[obj];
    }

    public void OnDragEnd(GameObject obj) {
        InventorySlot slot = inventoryItems[obj];

        if (mouseItem.hoverObj != null) { // Swap Item
            if (slot.item?.isMoveable == true && inventoryItems[mouseItem.hoverObj].item?.isMoveable != false) { // Cannot move an immovable item
                inventory.SwapItems(slot, inventoryItems[mouseItem.hoverObj]);
                player.hotBar.NotifySwap(slot.item, inventoryItems[mouseItem.hoverObj].item);
                SFXManager.instance.Play("Move Item", 0.9f, 1.1f);
            }
        } else { // Drop Item
            if (slot.item?.isMoveable == true) { // Cannot drop an immovable item
                ItemObject temp = slot.item;
                player.DropItem(slot);
                inventory.Remove(slot);
                player.hotBar.NotifyDrop(temp);
                SFXManager.instance.Play("Move Item", 0.9f, 1.1f);
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

    public void CleanUp() {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
        Destroy(mouseItem.obj);
        mouseItem.item = null;
        ClearInfo();
    }

    private void ClearInfo() {
        if (currentItemInfo != null) {
            Destroy(currentItemInfo.gameObject);
        }
    }
}

public class MouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}