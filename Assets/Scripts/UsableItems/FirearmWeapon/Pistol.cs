using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : UsableItem
{
	private const float DAMAGE = 20f;
	private const float RANGE = 100f;
	[SerializeField] string shootAnimation = "PistolFire";
	
	public ItemObject ammo;

	private HotBar hotBar;
	private Camera mainCamera;
	private Animator animator;
	private int layers;

	protected override void Init() {
		hotBar = GameObject.FindObjectOfType<HotBar>();
        mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		animator = GameObject.FindObjectOfType<ItemSwitching>().transform.gameObject.GetComponent<Animator>();
		ShowCrosshair();
    }
    
    protected override void Use() {
		if (hotBar.inventory.Has(ammo, 1)) {
			Shoot();
			hotBar.HandleItemUse(ammo);
			SFXManager.instance.Play("Pistol Shot", 0.9f, 1.1f);
		} else {
			Debug.Log("No Pistol Ammo");
			SFXManager.instance.Play("Gun Empty");
		}
    }

    private void Shoot() {
		RaycastHit hit;
		animator.Play(shootAnimation);
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, RANGE, ~layers))
		{
			Target target = hit.transform.GetComponent<Target>();
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			target?.TakeDamage(DAMAGE);
			enemy?.TakeDamage(DAMAGE);
		}
	}
}
