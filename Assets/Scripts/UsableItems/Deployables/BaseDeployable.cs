using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDeployable : UsableItem
{
    protected const float PLACE_DISTANCE = 6f;

    protected Player player;
    protected HotBar hotBar;
    protected Camera fpsCam;

    public string deploySoundEffect = "Default Deploy";
    public GameObject deployedPrefab;
    protected GameObject deployedPreview = null;

    protected void Update() {
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

    protected void InitPreview() {
        deployedPreview = Instantiate(deployedPrefab);
        foreach (MeshRenderer renderer in deployedPreview.GetComponentsInChildren<MeshRenderer>()) {
            renderer.material.color = new Color(1f, 1f, 1f, 0.5f);
        }
        UpdatePosition(deployedPreview);
    }

    protected override void Use() {
        Deploy();
        SFXManager.instance.Play(deploySoundEffect, 0.9f, 1.1f);
        hotBar.HandleItemUse(itemObject);
    }

    protected virtual void Deploy() {
        if (deployedPreview != null) { Destroy(deployedPreview); }
        GameObject inst = Instantiate(deployedPrefab);
        UpdatePosition(inst);
        inst.GetComponent<DeployedStatus>().isActive = true;
    }

    protected void UpdatePosition(GameObject inst) {
        Vector3 placePosition = player.transform.position + (PLACE_DISTANCE * fpsCam.transform.forward);
        placePosition.y = 0.2f;
        inst.transform.position = placePosition;
    }

    protected void OnDestroy() {
        if (deployedPreview != null) { Destroy(deployedPreview); }
    }
}
