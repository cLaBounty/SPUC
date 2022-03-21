using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : UsableItem
{
	private const float DAMAGE = 10f;
	private const float RANGE = 100f;

	private Camera fpsCam;
	private int layers;

	public override void Init() {
        fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
    }
    
    public override void Use() {
		if (fpsCam == null) { Init(); }
        Shoot();
    }

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
