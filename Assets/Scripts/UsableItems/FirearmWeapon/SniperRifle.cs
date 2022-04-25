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

	private float defaultFOV;
	private float coolDownTime;

	public ItemObject ammo;

	private Camera mainCamera;
	private Camera fpsCamera;
	private int layers;
	private Animator animator;
	private GameObject scopeOverlay;

	private float defaultFOV;
	private float coolDownTime;

	protected override void Init() {
        mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		fpsCamera = GameObject.FindObjectOfType<CameraSystem>().getCamera("FPSCam");
		layers = LayerMask.GetMask("Player");
		animator = GameObject.FindObjectOfType<ItemSwitching>().transform.gameObject.GetComponent<Animator>();
		scopeOverlay = GameObject.FindWithTag("SniperScope").transform.GetChild(0).gameObject;

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
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range, ~layers))
		{
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			enemy?.TakeDamage(player.damageMultiplier * damage);
		}
	}

	private IEnumerator OnScoped() {
		yield return new WaitForSeconds(0.15f);
		
		scopeOverlay.SetActive(true);
		fpsCamera.transform.gameObject.SetActive(false);
		mainCamera.fieldOfView = scopedFOV;
		MouseLook.SensitivityMultiplier = 0.5f;
	}

	private void OnUnscoped() {
		scopeOverlay.SetActive(false);
		fpsCamera.transform.gameObject.SetActive(true);
		mainCamera.fieldOfView = defaultFOV;
		MouseLook.SensitivityMultiplier = 1f;
	}

	private void OnDestroy() {
		animator.SetBool("IsSniperScoped", false);
		OnUnscoped();
	}
}
