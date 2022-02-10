using UnityEngine;

// Source: https://www.youtube.com/watch?v=THnivyG0Mvo
public class Gun : MonoBehaviour
{
	public float damage = 10f;
	public float range = 100f;

	public Camera fpsCam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
		{
			shoot();
		}
    }

	void shoot()
	{
		RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
		{
			Debug.Log(hit.transform.name);

			Target target = hit.transform.GetComponent<Target>();
			if (target != null)
			{
				target.TakeDamage(damage);
			}
		}
	}
}
