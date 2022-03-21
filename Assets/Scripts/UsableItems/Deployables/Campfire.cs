using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : UsableItem
{
    private const float PLACE_DISTANCE = 5f;

    private Player player;
    private Camera fpsCam;

    public GameObject placedPrefab;
    
    public override void Init() {
        player = GameObject.FindObjectOfType<Player>();
        fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
    }

    public override void Use() {
        if (fpsCam == null) { Init(); }
        Deploy();
    }

    private void Deploy() {
        var inst = Instantiate(placedPrefab);
        Vector3 placePosition = player.transform.position + (PLACE_DISTANCE * fpsCam.transform.forward);
        placePosition.y = 0.2f;
        inst.transform.position = placePosition;
    }
}
