using UnityEngine;

public class ResourceBeam : UsableItem
{
	private const float DAMAGE = 10f;
	private const float RANGE = 50f;
	private const float COLLECTION_RATE = 1f;

	private InventoryObject inventory;
	private Camera mainCamera;
	private int layers;

	protected override void Init() {
		inventory = Resources.Load<InventoryObject>("Inventory/PlayerInventory");
		mainCamera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		ShowCrosshair();
	}
    
    protected override void Use() {
		Shoot();
    }

    private void Shoot() {
		RaycastHit hit;
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, RANGE, ~layers))
		{
			ResourceNode rNode = hit.transform.GetComponent<ResourceNode>();
			if (rNode == null) return;

			int amount = rNode.harvest(DAMAGE);
			if (amount > 0)
			{
				inventory.AddItem(rNode.item, amount);
			}
		}
	}
}
