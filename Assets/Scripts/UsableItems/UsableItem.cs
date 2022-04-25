using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UsableItem : MonoBehaviour
{
    [SerializeField] protected ItemObject itemObject;

    protected Player player;
    protected HotBar hotBar;
    private GameObject crosshair;

    protected virtual void Init() {}
    protected virtual void Focus() {}
    protected abstract void Use();

    protected void Start() {
        player = GameObject.FindObjectOfType<Player>();
        hotBar = GameObject.FindObjectOfType<HotBar>();
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        Init();
    }

    protected void Update() {
        if (InventoryCanvas.InventoryIsOpen || PauseMenu.GameIsPaused) { return; }
        if (Input.GetButtonDown("Fire2")) { Focus(); }
        else if (Input.GetButtonDown("Fire1")) { Use(); }
    }

    protected void HideCrosshair() {
        crosshair.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    protected void ShowCrosshair() {
        crosshair.transform.GetComponent<Image>().color = new Color32(0, 0, 0, 110);
    }
}
