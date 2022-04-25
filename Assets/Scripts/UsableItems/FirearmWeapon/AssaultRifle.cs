using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : UsableItem
{
	private string shootAnimation = "AssultRifleFire";

	private const float DAMAGE = 35f;
	private const float RANGE = 100f;
	private const float COOL_DOWN = 0.25f;

	public ItemObject ammo;

	private HotBar hotBar;
	private Camera mainCamera;
	private Animator animator;
	private int layers;

	private float coolDownTime = COOL_DOWN;

	protected override void Init() {
		hotBar = GameObject.FindObjectOfType<HotBar>();
        mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		animator = GameObject.FindObjectOfType<ItemSwitching>().transform.gameObject.GetComponent<Animator>();
		ShowCrosshair();
    }

	private void Update() {
		coolDownTime += Time.deltaTime;
		if (InventoryCanvas.InventoryIsOpen || PauseMenu.GameIsPaused) { return; }
		if (Input.GetButtonDown("Fire2")) { Focus(); }
		if (Input.GetButton("Fire1")) {
			if (coolDownTime >= COOL_DOWN) {
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
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, RANGE, ~layers))
		{
			Target target = hit.transform.GetComponent<Target>();
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			target?.TakeDamage(DAMAGE);
			enemy?.TakeDamage(DAMAGE);
		}
	}
}
