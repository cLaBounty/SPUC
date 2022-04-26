using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : UsableItem
{
	[Header("Mechanics")]
	[SerializeField] private float damage = 20f;
    [SerializeField] private float range = 75f;

	[Header("Effects")]
	[SerializeField] string shootAnimation = "PistolFire";
	[SerializeField] ParticleSystem impactEffect;
	[SerializeField] Transform firePoint;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] TrailRenderer bulletTrail;


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
			muzzleFlash.Play();
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
			TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
			StartCoroutine(SpawnTrail(trail, hit));

			Enemy enemy = hit.transform.GetComponent<Enemy>();
			enemy?.TakeDamage(player.damageMultiplier * damage);

			if (enemy == null) return;

			Vector3 dir = firePoint.position - enemy.transform.position;
			impactEffect.transform.rotation = Quaternion.LookRotation(dir);
			impactEffect.transform.position = enemy.transform.position + dir.normalized * .5f;
			impactEffect.Play();
		}
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
