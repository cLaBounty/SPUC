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
        if (deployedPreview == null) { InitPreview(); }
        else { UpdatePosition(deployedPreview); }
    }

    protected override void Init() {
        player = GameObject.FindObjectOfType<Player>();
        hotBar = GameObject.FindObjectOfType<HotBar>();
        fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
        InitPreview();
        HideCrosshair();
    }

    private void InitPreview() {
        deployedPreview = Instantiate(deployedPrefab);
        foreach (MeshRenderer renderer in deployedPreview.GetComponentsInChildren<MeshRenderer>()) {
            renderer.material.color = new Color(1f, 1f, 1f, 0.5f);
        }
        UpdatePosition(deployedPreview);
    }

    protected override void Use() {
        Deploy();
        SFXManager.instance.Play("Fire Deploy", 0.9f, 1.1f);
        hotBar.HandleItemUse(itemObject);
    }

    private void Deploy() {
        if (deployedPreview != null) { Destroy(deployedPreview); }
        GameObject inst = Instantiate(deployedPrefab);
        UpdatePosition(inst);
        inst.GetComponent<DeployedCampfire>().isActive = true;
        inst.GetComponent<AudioSource>()?.Play();
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
