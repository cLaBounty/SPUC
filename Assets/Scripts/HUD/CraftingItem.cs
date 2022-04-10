using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingItem : MonoBehaviour, IPointerClickHandler {

    private DisplayCrafting craftingDisplay;

    private void Start() {
        craftingDisplay = GameObject.FindObjectOfType<DisplayCrafting>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        craftingDisplay.OnClick(transform.gameObject, eventData);
    }
}