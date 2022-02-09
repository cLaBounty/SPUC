using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: https://www.youtube.com/watch?v=kdckcSwPkrg
public class HotBar : MonoBehaviour
{
    private void Awake() {
        foreach(var button in GetComponentsInChildren<HotBarButton>()) {
            button.OnButtonClicked += ButtonOnButtonClicked;
        }
    }

    private void ButtonOnButtonClicked(int buttonNumber) {
        ItemSelector.SelectedItemIndex = buttonNumber - 1;
        Debug.Log($"Button {buttonNumber} clicked!");
    }
}

public static class ItemSelector
{
    public static int SelectedItemIndex { get; set; }
}