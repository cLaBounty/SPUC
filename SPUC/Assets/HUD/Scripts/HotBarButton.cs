using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotBarButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    
    public event Action<int> OnButtonClicked;

    private KeyCode keyCode;
    private int keyNumber;

    private void OnValidate() {
        keyNumber = transform.GetSiblingIndex() + 1;
        keyCode = KeyCode.Alpha0 + keyNumber;

        if (text == null) {
            text = GetComponentInChildren<TMP_Text>();
        }

        text.SetText(keyNumber.ToString());
        gameObject.name = "HotBar Button " + keyNumber;
    }

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void Update() {
        if (Input.GetKeyDown(keyCode)) {
            HandleClick();
        }
    }

    private void HandleClick() {
        OnButtonClicked?.Invoke(keyNumber);
    }
}
