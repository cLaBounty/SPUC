using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : UsableItem
{
	[Header("Mechanics")]
	[SerializeField] private float damage = 100f;
    [SerializeField] private float range = 150f;
    [SerializeField] private float coolDown = 0.25f;
	[SerializeField] private float scopedFOV = 15f;

	[Header("Effects")]
	[SerializeField] private string shootAnimation = "SniperFire";
	[SerializeField] ParticleSystem impactEffect;
	[SerializeField] Transform firePoint;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] TrailRenderer bulletTrail;

	public ItemObject ammo;

	private Camera mainCamera;
	private Camera fpsCamera;
	private int layers;
	private Animator animator;
	private GameObject scopeOverlay;

	private int layerIgnore;
	private int layerFPSCam;

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
	}

	protected override void Focus() {
		bool previousValue = animator.GetBool("IsSniperScoped");
		animator.SetBool("IsSniperScoped", !previousValue);
		if (!previousValue) { StartCoroutine(OnScoped()); }
		else { OnUnscoped(); }
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
			SFXManager.instance.Play("Sniper Shot", 0.9f, 1.1f);
		}
    }

    private void Shoot() {
		RaycastHit hit;
		muzzleFlash.Play();
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

	private IEnumerator OnScoped() {
		yield return new WaitForSeconds(0.15f);
		scopeOverlay.SetActive(true);
		gameObject.layer = layerIgnore;
		mainCamera.fieldOfView = scopedFOV;
		MouseLook.SensitivityMultiplier = 0.25f;
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

	private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint) {
		float time = 0;
		Vector3 startPos = trail.transform.position;
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
