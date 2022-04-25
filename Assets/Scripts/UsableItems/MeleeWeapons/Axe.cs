using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : UsableItem
{
	[SerializeField] private float damage = 10f;
    [SerializeField] private float range = 4f;

    private Camera mainCamera;
	private int layers;

    protected override void Init() {
        mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		ShowCrosshair();
    }
    
    protected override void Use() {
		SFXManager.instance.Play("Woosh", 0.8f, 1.2f);
        Melee();
    }

    // ToDo: implement melee attack, not raycast hit
    private void Melee() {
		RaycastHit hit;
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range, ~layers))
		{
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			enemy?.TakeDamage(player.damageMultiplier * damage);
		}
	}
}
