using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : UsableItem
{
	private const float DAMAGE = 20f;
	private const float RANGE = 100f;

	public ItemObject ammo;

	private HotBar hotBar;
	private Camera mainCamera;
	private int layers;

	protected override void Init() {
		hotBar = GameObject.FindObjectOfType<HotBar>();
        mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		ShowCrosshair();
    }
    
    protected override void Use() {
		if (hotBar.inventory.Has(ammo, 1)) {
			Shoot();
			hotBar.HandleItemUse(ammo);
		} else {
			Debug.Log("No Pistol Ammo");
		}
    }

    private void Shoot() {
		RaycastHit hit;
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, RANGE, ~layers))
		{
			Target target = hit.transform.GetComponent<Target>();
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			target?.TakeDamage(DAMAGE);
			enemy?.TakeDamage(DAMAGE);
		}
	}
}
