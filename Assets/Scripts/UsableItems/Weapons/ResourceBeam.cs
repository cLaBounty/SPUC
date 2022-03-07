using UnityEngine;

public class ResourceBeam : UsableItem
{
	[SerializeField] private float damage = 10f;
	[SerializeField] private float range = 100f;
	[SerializeField] private float collectionRate = 1f;

	public InventoryObject inventory;
	public Camera fpsCam;
	private int layers;

	private void Start() {
		inventory = Resources.Load<InventoryObject>("PlayerInventory");
		fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
	}
    
    public override void Use() {
		Start(); // ToDo: move to an on create function

        Shoot();
    }

    void Shoot() {
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
