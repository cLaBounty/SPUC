using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public static int WavesCompleted = 0;
    public static int EnemiesKilled = 0;
    public static float DamageDealt = 0f;
    public static float DamageTaken = 0f;

    [SerializeField] float itemGrav = 1f;
    [SerializeField] float itemRange = 2f;
	[SerializeField] float vignetteTime = 0.25f;
    [SerializeField] Volume damageVignette;
    [SerializeField] float vignetteSpeed = 10f;

    public float initialHealth = 100f;
    public float currentHealth;
    public float maxHealth;
    public float defense = 0f;
    public float damageMultiplier = 1f;

    public HealthBar healthBar;
    public HotBar hotBar;
    public InventoryObject inventory;
    public CraftingObject crafting;
    private CameraSystem cameraSystem;

    const float ITEM_DROP_DISTANCE = 8f;

    bool hurtEffect = false;
    float hurtEffectLerp = 0;

    LayerMask itemMask;

    private void Start()
    {
        currentHealth = initialHealth;
        maxHealth = initialHealth;
        healthBar.SetMaxHealth(initialHealth);
        healthBar.valueText.gameObject.SetActive(true);
        cameraSystem = GameObject.FindObjectOfType<CameraSystem>();
        itemMask = LayerMask.GetMask("Ground Item");
    }

    void Update() {
        //move objects towards player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, itemRange, itemMask);
        Vector2 totalForce = Vector2.zero;

        foreach(Collider col in hitColliders){
            col.transform.parent.position += (transform.position - col.transform.parent.position).normalized * itemGrav * Time.deltaTime;
        }

        //hurt effect
        if (hurtEffect){
            hurtEffectLerp += Time.deltaTime * vignetteSpeed;
            hurtEffectLerp = Mathf.Clamp(hurtEffectLerp, 0, 1);
            damageVignette.weight = Mathf.Cos(hurtEffectLerp * Mathf.PI * 2 + Mathf.PI) * 0.5f + 0.5f;

            if (hurtEffectLerp == 1){
                hurtEffect = false;
                damageVignette.gameObject.SetActive(false);
            }
        }
    }

    public List<Enemy> GetLivingRobots() {
        List<Enemy> livingRobots = new List<Enemy>();
        DeployedEnemyPuncher[] punchers = GameObject.FindObjectsOfType<DeployedEnemyPuncher>();
        foreach(DeployedEnemyPuncher puncher in punchers) {
            livingRobots.Add(puncher);
        }
        DeployedEnemyShooter[] shooters = GameObject.FindObjectsOfType<DeployedEnemyShooter>();
        foreach(DeployedEnemyShooter shooter in shooters) {
            livingRobots.Add(shooter);
        }
        DeployedOilHealer[] healers = GameObject.FindObjectsOfType<DeployedOilHealer>();
        foreach(DeployedOilHealer healer in healers) {
            livingRobots.Add(healer);
        }
        return livingRobots;
    }

    public void IncreaseMaxHealth(float value) {
        maxHealth += value;
        healthBar.UpdateMaxHealth(maxHealth);
        healthBar.valueText.text = currentHealth.ToString("n0") + "/" + maxHealth.ToString("n0");
    }

    public void TakeDamage(float amount)
    {
        float damage = Mathf.Max(1f, amount - defense);
        if (damage > currentHealth) {
            DamageTaken += currentHealth;
            currentHealth = 0;
        } else {
            DamageTaken += damage;
            currentHealth -= damage;
        }
        healthBar.SetHealth(currentHealth);
        healthBar.valueText.text = currentHealth.ToString("n0") + "/" + maxHealth.ToString("n0");

        // Hurt Visual Effect
        hurtEffect = true;
		hurtEffectLerp = 0;
        damageVignette.gameObject.SetActive(true);

        // Hurt Sound Effect
        SFXManager.instance?.Play("Hurt");
    }

    public void GainHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        
        healthBar.SetHealth(currentHealth);
        healthBar.valueText.text = currentHealth.ToString("n0") + "/" + maxHealth.ToString("n0");
    }

    // Inventory
    public void PickUpItem(GroundItem groundItem) {
        inventory.AddItem(groundItem.item, groundItem.amount);
    }

    public void DropItem(InventorySlot slot) {
        var inst = Instantiate(slot.item.groundPrefab);
        inst.GetComponent<GroundItem>().amount = slot.amount;
        
        Vector3 dropPosition = transform.position + (ITEM_DROP_DISTANCE * cameraSystem.getMainCamera().transform.forward);
        dropPosition.y = 0.5f; // Fixes issue of items dropping underground
        inst.transform.position = dropPosition;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "GroundItem") {
            GroundItem gi = other.gameObject.GetComponent<GroundItem>();
            SFXManager.instance?.Play(gi.sfx, 0.9f, 1.1f);
            PickUpItem(gi);
            if (gi.currentInfo != null) { Destroy(gi.currentInfo.gameObject); }
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "EnemyProjectile") {
            TakeDamage(other.gameObject.GetComponent<EnemyProjectile>().damage);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "GroundItem") {
            GroundItem gi = other.gameObject.GetComponent<GroundItem>();
            SFXManager.instance?.Play(gi.sfx, 0.9f, 1.1f);
            PickUpItem(gi);
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "EnemyProjectile") {
            TakeDamage(other.gameObject.GetComponent<EnemyProjectile>().damage);
        }
    }

    public void CleanUp() {
        inventory.container.items = new InventorySlot[28];
        crafting.container.items = new InventorySlot[28];
    }

    private void OnApplicationQuit() {
        CleanUp();
    }
}
