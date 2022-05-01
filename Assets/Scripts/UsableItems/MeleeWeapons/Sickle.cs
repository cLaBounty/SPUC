using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickle : UsableItem
{
	[SerializeField] string swingAnimation = "MeleeSwing";

	[Header("Mechanics")]
	[SerializeField] private float damage = 25f;
    [SerializeField] private float range = 4f;

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
		SFXManager.instance.Play("Woosh", 0.8f, 1.2f);
		animator.Play(swingAnimation);
    }
	
    private void Melee() {
		RaycastHit hit;
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range, ~layers))
		{
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			enemy?.TakeDamage(player.damageMultiplier * damage);
		}
	}
}
