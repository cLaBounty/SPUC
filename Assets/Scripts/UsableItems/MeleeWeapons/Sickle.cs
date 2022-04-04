using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickle : UsableItem
{
    private const float DAMAGE = 25f;
	private const float RANGE = 4f;

    private Camera fpsCam;
	private int layers;

    protected override void Init() {
        fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		ShowCrosshair();
    }
    
    protected override void Use() {
        Melee();
    }

    // ToDo: implement melee attack, not raycast hit
    private void Melee() {
		RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, RANGE, ~layers))
		{
			Target target = hit.transform.GetComponent<Target>();
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			target?.TakeDamage(DAMAGE);
			enemy?.TakeDamage(DAMAGE);
		}
	}
}
