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

	[SerializeField] ParticleSystem impactEffect;
	[SerializeField] Transform firePoint;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] TrailRenderer bulletTrail;

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
			muzzleFlash.Play();
			hotBar.HandleItemUse(ammo);
			SFXManager.instance.Play("Pistol Shot", 0.9f, 1.1f); // ToDo: replace with AR sound effect
		}
    }

    private void Shoot() {
		RaycastHit hit;
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, RANGE, ~layers))
		{
			TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
			StartCoroutine(SpawnTrail(trail, hit));
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			enemy?.TakeDamage(DAMAGE);
			if (enemy == null)
				return;
			Vector3 dir = firePoint.position - enemy.transform.position;
			impactEffect.transform.rotation = Quaternion.LookRotation(dir);
			impactEffect.transform.position = enemy.transform.position + dir.normalized * .5f;
			impactEffect.Play();
		}

		
	}

	private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit) {
		float time = 0;
		Vector3 startPos = firePoint.transform.position;
		while (time < 1)
		{
			trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
			time += Time.deltaTime / trail.time;

			yield return null;
		}
		trail.transform.position = hit.point;

		Destroy(trail.gameObject, trail.time);
	}
}
