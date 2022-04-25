using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : UsableItem
{
	[SerializeField] string shootAnimation = "PistolFire";
	[SerializeField] private float damage = 20f;
    [SerializeField] private float range = 75f;

	public ItemObject ammo;

	private Camera mainCamera;
	private Animator animator;
	private int layers;

	protected override void Init() {
        mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		animator = GameObject.FindObjectOfType<ItemSwitching>().transform.gameObject.GetComponent<Animator>();
		ShowCrosshair();
    }
    
    protected override void Use() {
		if (hotBar.inventory.Has(ammo, 1)) {
			Shoot();
			animator.Play(shootAnimation);
			hotBar.HandleItemUse(ammo);
			SFXManager.instance.Play("Pistol Shot", 0.9f, 1.1f);
		} else {
			SFXManager.instance.Play("Gun Empty");
		}
    }

    private void Shoot() {
		RaycastHit hit;
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range, ~layers))
		{
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			enemy?.TakeDamage(player.damageMultiplier * damage);
		}
	}
}
