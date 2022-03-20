using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : UsableItem
{
	private const float DAMAGE = 100f;
    private const float RANGE = 150f;

	private Camera fpsCam;
	private int layers;

	public override void Init() {
        fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		IsInitted = true;
    }
    
    public override void Use() {
		if (!IsInitted) { Init(); }
        Shoot();
    }

    // ToDo: implement... currently same as pistol
    private void Shoot() {
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
