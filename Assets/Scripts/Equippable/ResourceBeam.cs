using UnityEngine;

public class ResourceBeam : MonoBehaviour
{
	//public ItemObject item;

	public InventoryObject inventory;

	[SerializeField] float damage = 10f;
	[SerializeField] float range = 100f;
	[SerializeField] float collectionRate = 1f;

	private int layers;

	public Camera fpsCam;

	private void Start() {
		layers = LayerMask.GetMask("Player");
	}

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
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, ~layers))
		{
			//Debug.Log(hit.transform.name);

			ResourceNode rNode = hit.transform.GetComponent<ResourceNode>();
			if (rNode == null) return;

			int amount = rNode.harvest(damage);
			if (amount > 0)
			{
				inventory.AddItem(rNode.item, amount);
				Debug.Log($"{rNode.item.name} collected!");
			}
            
		}
	}
}