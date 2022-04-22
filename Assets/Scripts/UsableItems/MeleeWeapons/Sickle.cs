using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickle : UsableItem
{
    private const float DAMAGE = 25f;
	private const float RANGE = 4f;
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
		SFXManager.instance.Play("Woosh", 0.8f, 1.2f);
        Melee();
    }

    // ToDo: implement melee attack, not raycast hit
    private void Melee() {
		RaycastHit hit;
		animator.Play(swingAnimation, -1, 0f);
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, RANGE, ~layers))
		{
			Target target = hit.transform.GetComponent<Target>();
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			target?.TakeDamage(DAMAGE);
			enemy?.TakeDamage(DAMAGE);
		}
	}
}
