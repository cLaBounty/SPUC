using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : UsableItem
{
    private const float PLACE_DISTANCE = 6f;

    private Player player;
    private HotBar hotBar;
    private Camera fpsCam;

    public GameObject deployedPrefab;
    private GameObject deployedPreview = null;

    private void Update() {
        base.Update();
        if (deployedPreview != null) { UpdatePosition(deployedPreview); }
    }

    protected override void Init() {
        player = GameObject.FindObjectOfType<Player>();
        hotBar = GameObject.FindObjectOfType<HotBar>();
        fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();

        deployedPreview = Instantiate(deployedPrefab);
        foreach (MeshRenderer renderer in deployedPreview.GetComponentsInChildren<MeshRenderer>()) {
            renderer.material.color = new Color(1f, 1f, 1f, 0.5f);
        }
        UpdatePosition(deployedPreview);
        
        HideCrosshair();
    }

    protected override void Use() {
        Deploy();
        hotBar.HandleItemUse(itemObject);
    }

    private void Deploy() {
        if (deployedPreview != null) { Destroy(deployedPreview); }
        GameObject inst = Instantiate(deployedPrefab);
        UpdatePosition(inst);
        inst.GetComponent<DeployedCampfire>().isActive = true;
    }

    private void UpdatePosition(GameObject inst) {
        Vector3 placePosition = player.transform.position + (PLACE_DISTANCE * fpsCam.transform.forward);
        placePosition.y = 0.2f;
        inst.transform.position = placePosition;
    }

    private void OnDestroy() {
        if (deployedPreview != null) { Destroy(deployedPreview); }
    }
}
