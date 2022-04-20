using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : UsableItem
{
	private const float DAMAGE = 100f;
    private const float RANGE = 150f;
	private const float COOL_DOWN = 1f;
	private const float SCOPED_FOV = 15f;
	private float defaultFOV;

	public ItemObject ammo;

	private HotBar hotBar;
	private Camera mainCamera;
	private Camera fpsCamera;
	private int layers;
	private Animator animator;
	private GameObject scopeOverlay;

	private float coolDownTime = COOL_DOWN;

	protected override void Init() {
		hotBar = GameObject.FindObjectOfType<HotBar>();
        mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		defaultFOV = mainCamera.fieldOfView;
		fpsCamera = GameObject.FindObjectOfType<CameraSystem>().getCamera("FPSCam");
		layers = LayerMask.GetMask("Player");
		animator = GameObject.FindObjectOfType<ItemSwitching>().transform.gameObject.GetComponent<Animator>();
		scopeOverlay = GameObject.FindWithTag("SniperScope").transform.GetChild(0).gameObject;
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
		if (coolDownTime >= COOL_DOWN) { coolDownTime = 0; }
		else { return; }

		if (hotBar.inventory.Has(ammo, 1)) {
			Shoot();
			hotBar.HandleItemUse(ammo);
		} else {
			Debug.Log("No Sniper Ammo");
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

	private IEnumerator OnScoped() {
		yield return new WaitForSeconds(0.15f);
		
		scopeOverlay.SetActive(true);
		fpsCamera.transform.gameObject.SetActive(false);
		mainCamera.fieldOfView = SCOPED_FOV;
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
