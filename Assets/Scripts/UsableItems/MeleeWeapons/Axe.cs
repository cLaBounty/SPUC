using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : UsableItem
{
	[SerializeField] private float damage = 10f;
    [SerializeField] private float range = 4f;
	private string swingAnimation = "MeleeSwing";

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
		SFXManager.instance.Play("Woosh", 0.8f, 1.2f);
    animator.Play(swingAnimation);
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
