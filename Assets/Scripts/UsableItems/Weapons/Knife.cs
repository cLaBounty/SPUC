using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : UsableItem
{
    private const float DAMAGE = 15f;
	private const float RANGE = 3f;

    private Camera fpsCam;
	private int layers;

    public override void Init() {
        fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
        IsInitted = true;
    }
    
    public override void Use() {
        if (!IsInitted) { Init(); }
        Melee();
    }

    // ToDo: implement melee attack, not raycast hit
    private void Melee() {
		RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, RANGE, ~layers))
		{
			Target target = hit.transform.GetComponent<Target>();
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			target?.TakeDamage(DAMAGE);
			enemy?.TakeDamage(DAMAGE);
		}
	}
}
