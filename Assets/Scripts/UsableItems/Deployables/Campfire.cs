using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : UsableItem
{
    private const float PLACE_DISTANCE = 5f;

    private Player player;
    private HotBar hotBar;
    private Camera fpsCam;

    public GameObject placedPrefab;
    
    public override void Init() {
        player = GameObject.FindObjectOfType<Player>();
        hotBar = GameObject.FindObjectOfType<HotBar>();
        fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
    }

    public override void Use() {
        if (hotBar == null) { Init(); }
        Deploy();
        hotBar.HandleItemUse(itemObject);
    }

    private void Deploy() {
        var inst = Instantiate(placedPrefab);
        Vector3 placePosition = player.transform.position + (PLACE_DISTANCE * fpsCam.transform.forward);
        placePosition.y = 0.2f;
        inst.transform.position = placePosition;
    }
}
