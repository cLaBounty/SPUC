using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : UsableItem
{
	[Header("Mechanics")]
	[SerializeField] private float damage = 35f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float coolDown = 0.25f;

	[Header("Effects")]
	[SerializeField] private string shootAnimation = "AssultRifleFire";
	[SerializeField] ParticleSystem impactEffect;
	[SerializeField] Transform firePoint;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] TrailRenderer bulletTrail;

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
		if (InventoryCanvas.InventoryIsOpen || PauseMenu.GameIsPaused) { return; }
		if (Input.GetButtonDown("Fire2")) { Focus(); }
		if (Input.GetButton("Fire1")) { Use(); } // 
		coolDownTime += Time.deltaTime;
	}
    
    protected override void Use() {
		if (Input.GetButtonDown("Fire1") && !hotBar.inventory.Has(ammo, 1)) {
			SFXManager.instance.Play("Gun Empty");
		}

		if (coolDownTime >= coolDown) { coolDownTime = 0; }
		else { return; }

		if (hotBar.inventory.Has(ammo, 1)) {
			Shoot();
			animator.Play(shootAnimation);
			hotBar.HandleItemUse(ammo);
			SFXManager.instance.Play("Machine Gun", 0.9f, 1.1f);
		}
    }

    private void Shoot() {
		RaycastHit hit;
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range, ~layers))
		{
			TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
			StartCoroutine(SpawnTrail(trail, hit.point));

			Enemy enemy = hit.transform.GetComponent<Enemy>();
			enemy?.TakeDamage(player.damageMultiplier * damage);

			if (enemy == null) return;
			
			Vector3 dir = firePoint.position - enemy.transform.position;
			impactEffect.transform.rotation = Quaternion.LookRotation(dir);
			impactEffect.transform.position = hit.point + dir.normalized;
			impactEffect.Play();
		} else
		{
			TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
			StartCoroutine(SpawnTrail(trail, firePoint.position + mainCamera.transform.forward * 30));
		}
	}

	private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint) {
		float time = 0;
		Vector3 startPos = firePoint.transform.position;
		while (time < 1)
		{
			trail.transform.position = Vector3.Lerp(startPos, hitPoint, time);
			time += Time.deltaTime / trail.time;

			yield return null;
		}
		trail.transform.position = hitPoint;

		Destroy(trail.gameObject, trail.time);
	}
}
