using UnityEngine;

public class ResourceBeam : UsableItem
{
	[SerializeField] private float damage = 10f;
	[SerializeField] private float range = 100f;
	[SerializeField] private float collectionRate = 1f;

	private InventoryObject inventory;
	private Camera fpsCam;
	private int layers;

	public override void Init() {
		inventory = Resources.Load<InventoryObject>("Inventory/PlayerInventory");
		fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		IsInitted = true;
	}
    
    public override void Use() {
		if (!IsInitted) { Init(); }
		Shoot();
    }

    private void Shoot() {
		RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, ~layers))
		{
			ResourceNode rNode = hit.transform.GetComponent<ResourceNode>();
			if (rNode == null) return;

			int amount = rNode.harvest(damage);
			if (amount > 0)
			{
				inventory.AddItem(rNode.item, amount);
			}
		}
	}
}
