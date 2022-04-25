using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : UsableItem
{
	[SerializeField] private string shootAnimation = "PistolFire"; // ToDo: replace with AR animation
	[SerializeField] private float damage = 35f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float coolDown = 0.25f;

	public ItemObject ammo;

	private Camera mainCamera;
	private Animator animator;
	private int layers;

	private float coolDownTime;

	protected override void Init() {
        mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		animator = GameObject.FindObjectOfType<ItemSwitching>().transform.gameObject.GetComponent<Animator>();
		coolDownTime = coolDown;
		ShowCrosshair();
    }

	private void Update() {
		coolDownTime += Time.deltaTime;
		if (InventoryCanvas.InventoryIsOpen || PauseMenu.GameIsPaused) { return; }
		if (Input.GetButtonDown("Fire2")) { Focus(); }
		if (Input.GetButton("Fire1")) {
			if (coolDownTime >= coolDown) {
				coolDownTime = 0;
				Use();
			}
		}

		// Empty effect should only play once
		if (Input.GetButtonDown("Fire1") && !hotBar.inventory.Has(ammo, 1)) {
			SFXManager.instance.Play("Gun Empty");
		}
	}
    
    protected override void Use() {
		if (hotBar.inventory.Has(ammo, 1)) {
			Shoot();
			animator.Play(shootAnimation);
			hotBar.HandleItemUse(ammo);
			SFXManager.instance.Play("Pistol Shot", 0.9f, 1.1f); // ToDo: replace with AR sound effect
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
