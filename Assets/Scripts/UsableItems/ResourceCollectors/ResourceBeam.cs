using UnityEngine;

public class ResourceBeam : UsableItem
{
	private const float DAMAGE = 10f;
	private const float RANGE = 50f;
	private const float COLLECTION_RATE = 1f;

	private InventoryObject inventory;
	private Camera fpsCam;
	private int layers;

	[Header("Laser")]
	public LineRenderer lineRenderer;
	public Transform firePoint;
	public ParticleSystem impactEffect;
	private float coolDownTime = 0;
	public float frequency = 1f;

	protected override void Init() {
		inventory = Resources.Load<InventoryObject>("Inventory/PlayerInventory");
		fpsCam = GameObject.FindObjectOfType<CameraSystem>().getMainCamera();
		layers = LayerMask.GetMask("Player");
		ShowCrosshair();
	}

	private void Update() {
		coolDownTime += Time.deltaTime;
		if (InventoryCanvas.InventoryIsOpen || PauseMenu.GameIsPaused) { return; }
			if (Input.GetButtonDown("Fire2")) { Focus(); }
				if (Input.GetButton("Fire1")) 
					Use();
				else
				{
					if (lineRenderer.enabled)
					{
						lineRenderer.enabled = false;
						impactEffect.Stop();
					}
				}
		}

    protected override void Use() {
		Shoot();
    }

    private void Shoot() {
		RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, RANGE, ~layers))
		{
			Laser(hit.point);
			if (coolDownTime >= frequency) 
			{
				coolDownTime = 0;
				ResourceNode rNode = hit.transform.GetComponent<ResourceNode>();
				if (rNode == null) 
				{
					impactEffect.Stop();
					return;
				}
				int amount = rNode.harvest(DAMAGE);
				if (amount > 0)
				{
					impactEffect.Play();
					inventory.AddItem(rNode.item, amount);
				} 
				else 
					impactEffect.Stop();
			}
		} 
		else
		{
			Vector3 target = firePoint.position + firePoint.forward * RANGE;
			Laser(target);
			impactEffect.Stop();
		}
	}

	private void Laser(Vector3 target) {
		if (!lineRenderer.enabled)
		{
			lineRenderer.enabled = true;
		}
		lineRenderer.SetPosition(0, firePoint.position);
		lineRenderer.SetPosition(1, target);
		
		Vector3 direction = firePoint.position - target;

		impactEffect.transform.position = target + direction.normalized;
		impactEffect.transform.rotation = Quaternion.LookRotation(direction);
	}
}
