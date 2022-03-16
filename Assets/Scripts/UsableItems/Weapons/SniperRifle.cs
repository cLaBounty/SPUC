using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : UsableItem
{
	[SerializeField] private float damage = 100f;
	[SerializeField] private float range = 150f;

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
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, ~layers))
		{
			Target target = hit.transform.GetComponent<Target>();
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			target?.TakeDamage(damage);
			enemy?.TakeDamage(damage);
		}
	}
}
