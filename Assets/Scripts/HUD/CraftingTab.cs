using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CraftingTab : MonoBehaviour
{
    [SerializeField] private bool isSelected;
    [SerializeField] private bool isAvailableTab;
    [SerializeField] private CraftingTab otherTab;

    private DisplayCrafting craftingDisplay;

    private void Start()
    {
        craftingDisplay = GameObject.FindObjectOfType<DisplayCrafting>();
        AddEvent(EventTriggerType.PointerClick, delegate { OnClick(); });
    }

    private void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action) {
        EventTrigger trigger = transform.gameObject.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnClick() {
        if (isSelected) return;
        Select();
        otherTab.Deselect();
        craftingDisplay.SetDisplayAvailableOnly(isAvailableTab);
    }

    private void Select() {
        isSelected = true;
        transform.gameObject.GetComponent<Image>().color = new Color32(137, 81, 44, 255);
    }

    public void Deselect() {
        isSelected = false;
        transform.gameObject.GetComponent<Image>().color = new Color32(178, 138, 96, 255);
    }
}
