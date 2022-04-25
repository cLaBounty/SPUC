using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : UsableItem
{
	[SerializeField] private string shootAnimation = "SniperFire";
	[SerializeField] private float damage = 35f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float coolDown = 0.25f;
	[SerializeField] private float scopedFOV = 15f;

	public ItemObject ammo;

	private Camera mainCamera;
	private Camera fpsCamera;
	private int layers;
	private int layerIgnore;
	private int layerFPSCam;
	private Animator animator;

	[SerializeField] ParticleSystem impactEffect;
	[SerializeField] Transform firePoint;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] TrailRenderer bulletTrail;

	private GameObject scopeOverlay;

	private float defaultFOV;
	private float coolDownTime;

	protected override void Init() {
        mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		fpsCamera = GameObject.FindObjectOfType<CameraSystem>().getCamera("FPSCam");
		layers = LayerMask.GetMask("Player");
		animator = GameObject.FindObjectOfType<ItemSwitching>().transform.gameObject.GetComponent<Animator>();
		scopeOverlay = GameObject.FindWithTag("SniperScope").transform.GetChild(0).gameObject;

		layerIgnore = LayerMask.NameToLayer("Ignore");
		layerFPSCam = LayerMask.NameToLayer("FPSCam");

		defaultFOV = mainCamera.fieldOfView;
		coolDownTime = coolDown;
		
		HideCrosshair();
    }

	private void Update() {
		base.Update();
		coolDownTime += Time.deltaTime;

		// Empty effect should only play without having to wait for cooldown
		if (Input.GetButtonDown("Fire1") && !hotBar.inventory.Has(ammo, 1)) {
			SFXManager.instance.Play("Gun Empty");
		}

	}

	protected override void Focus() {
		bool previousValue = animator.GetBool("IsSniperScoped");
		animator.SetBool("IsSniperScoped", !previousValue);
		if (!previousValue) { StartCoroutine(OnScoped()); }
		else { OnUnscoped(); }
	}
    
    protected override void Use() {
		if (coolDownTime >= coolDown) { coolDownTime = 0; }
		else { return; }

		if (hotBar.inventory.Has(ammo, 1)) {
			Shoot();
			animator.Play(shootAnimation);
			
			hotBar.HandleItemUse(ammo);
			SFXManager.instance.Play("Sniper Shot", 0.9f, 1.1f);
		} else {
			SFXManager.instance.Play("Gun Empty");
		}
    }

    private void Shoot() {
		RaycastHit hit;
		muzzleFlash.Play();
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range, ~layers))
		{
			TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
			StartCoroutine(SpawnTrail(trail, hit));
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			enemy?.TakeDamage(player.damageMultiplier * damage);
			if (enemy == null)
				return;
			Vector3 dir = firePoint.position - enemy.transform.position;
			impactEffect.transform.rotation = Quaternion.LookRotation(dir);
			impactEffect.transform.position = enemy.transform.position + dir.normalized * .5f;
			impactEffect.Play();
		}
	}

	private IEnumerator OnScoped() {
		yield return new WaitForSeconds(0.15f);
		scopeOverlay.SetActive(true);
		gameObject.layer = layerIgnore;
		mainCamera.fieldOfView = scopedFOV;
		MouseLook.SensitivityMultiplier = 0.5f;
	}

	private void OnUnscoped() {
		scopeOverlay.SetActive(false);
		gameObject.layer = layerFPSCam;
		mainCamera.fieldOfView = defaultFOV;
		MouseLook.SensitivityMultiplier = 1f;
	}

	private void OnDestroy() {
		animator.SetBool("IsSniperScoped", false);
		OnUnscoped();
	}

	private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit) {
		float time = 0;
		Vector3 startPos = trail.transform.position;
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
