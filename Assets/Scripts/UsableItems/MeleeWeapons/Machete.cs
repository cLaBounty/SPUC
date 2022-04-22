using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machete : UsableItem
{
    private const float DAMAGE = 20f;
	private const float RANGE = 6f;
	[SerializeField] string swingAnimation = "MeleeSwing";

	private Animator animator;
    private Camera mainCamera;
	private int layers;

    protected override void Init() {
        mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		animator = GameObject.FindObjectOfType<ItemSwitching>().transform.gameObject.GetComponent<Animator>();
		ShowCrosshair();
    }
    
    protected override void Use() {
        Melee();
    }

    // ToDo: implement melee attack, not raycast hit
    private void Melee() {
		RaycastHit hit;
		animator.Play(swingAnimation);
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, RANGE, ~layers))
		{
			Target target = hit.transform.GetComponent<Target>();
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			target?.TakeDamage(DAMAGE);
			enemy?.TakeDamage(DAMAGE);
		}
	}
}
