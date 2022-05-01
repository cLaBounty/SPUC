using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDeployable : UsableItem
{
    [SerializeField] private string deploySoundEffect = "Default Deploy";
    [SerializeField] private float placeDistance = 6f;
    
    public GameObject deployedPrefab;
    public GameObject deployedPreview = null;

    protected Camera fpsCam;
    
    protected void Update() {
        base.Update();
        if (deployedPreview == null) { InitPreview(); }
        else { UpdatePosition(deployedPreview); }
    }

    protected override void Init() {
        fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
        InitPreview();
        HideCrosshair();
    }

    protected void InitPreview() {
        deployedPreview = Instantiate(deployedPrefab);
        UpdatePosition(deployedPreview);
    }

    protected override void Use() {
        Deploy();
        SFXManager.instance.Play(deploySoundEffect, 0.9f, 1.1f);
        hotBar.HandleItemUse(itemObject);
    }

    protected virtual void Deploy() {
        Vector3 placePosition = player.transform.position + (placeDistance * fpsCam.transform.forward);
        placePosition.y = 0.2f;
        GameObject inst = Instantiate(deployedPrefab, placePosition, deployedPreview.transform.rotation);
        if (deployedPreview != null) { Destroy(deployedPreview); }
        inst.transform.parent = null;
        UpdatePosition(inst);
        inst.GetComponent<DeployedStatus>().isActive = true;
    }

    protected void UpdatePosition(GameObject inst) {
        Vector3 placePosition = player.transform.position + (placeDistance * fpsCam.transform.forward);
        placePosition.y = 0.2f;
        inst.transform.position = placePosition;
    }
}
