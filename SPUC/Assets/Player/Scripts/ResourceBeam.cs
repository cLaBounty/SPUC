using UnityEngine;

public class ResourceBeam : MonoBehaviour
{
	public InventoryObject inventory;
	[SerializeField] float damage = 10f;
	[SerializeField] float range = 100f;
	[SerializeField] float collectionRate = 1f;

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

			ResourceNode rNode = hit.transform.GetComponent<ResourceNode>();
			if (rNode == null) return;

			int amount = rNode.harvest(damage);
			if (amount > 0)
			{
				inventory.AddItem(new Item(rNode.item), amount);
				Debug.Log($"{rNode.item.name} collected!");
			}
            
		}
	}
}