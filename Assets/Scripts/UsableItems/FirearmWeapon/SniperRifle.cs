using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : UsableItem
{
	private const float DAMAGE = 100f;
    private const float RANGE = 150f;

	public ItemObject ammo;

	private HotBar hotBar;
	private Camera fpsCam;
	private int layers;

	protected override void Init() {
		hotBar = GameObject.FindObjectOfType<HotBar>();
        fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		HideCrosshair();
    }

	protected override void Focus() {
		// ToDo: toggle sniper rifle scope
	}
    
    protected override void Use() {
		if (hotBar.inventory.Has(ammo, 1)) {
			Shoot();
			hotBar.HandleItemUse(ammo);
		} else {
			Debug.Log("No Sniper Ammo");
		}
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
