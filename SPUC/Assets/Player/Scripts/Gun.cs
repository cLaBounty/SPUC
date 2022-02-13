using UnityEngine;

// Source: https://www.youtube.com/watch?v=THnivyG0Mvo
public class Gun : MonoBehaviour
{
	[SerializeField] float damage = 10f;
	[SerializeField] float range = 100f;

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
			Enemy enemy = hit.transform.GetComponent<Enemy>();

			target?.TakeDamage(damage);
			enemy?.TakeDamage(damage);
		}
	}
}
