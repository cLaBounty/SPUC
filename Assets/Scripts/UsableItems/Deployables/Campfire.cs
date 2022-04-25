using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : BaseDeployable
{
    protected override void Deploy() {
        if (deployedPreview != null) { Destroy(deployedPreview); }
        GameObject inst = Instantiate(deployedPrefab);
        UpdatePosition(inst);
        inst.GetComponent<DeployedStatus>().isActive = true;
        inst.GetComponent<AudioSource>()?.Play();
    }
}
