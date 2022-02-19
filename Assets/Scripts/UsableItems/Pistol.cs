using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : UsableItem
{
	[SerializeField] private float damage = 10f;
	[SerializeField] private float range = 100f;

	public Camera fpsCam;
	private int layers;

	private void Start() {
		layers = LayerMask.GetMask("Player");
	}
    
    public override void Use() {
        Shoot();
    }

    void Shoot() {
		RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, ~layers))
		{
			//Debug.Log(hit.transform.name);

			Target target = hit.transform.GetComponent<Target>();
			Enemy enemy = hit.transform.GetComponent<Enemy>();

			target?.TakeDamage(damage);
			enemy?.TakeDamage(damage);
		}
	}
}
