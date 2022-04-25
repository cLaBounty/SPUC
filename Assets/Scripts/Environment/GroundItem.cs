using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;
    public int amount = 1;
    public float spawnRate = 1f;
    public string sfx = "Gun Pickup";

    private float pickupDistance = 25f;
    public GameObject infoPrefab;
    private Player player;
    public GameObject currentInfo = null;

    // Bounce Effect
    private float hoverRate = 0.5f;
    private float highestOffset = 0.6f;
    private float startingZ;
    private float time = 0;

    //moving away from other objects
    [Header("Seperations")]
    public float stayAwayDist = 1f;
    public float moveAwaySpeed = 1f;
    //Collider colliderr;
    LayerMask mask;
    LayerMask ground;
    bool grounded = false;

    void Start() {
        player = GameObject.FindObjectOfType<Player>();

        // Bounce Effect
        startingZ = transform.position.y;
        time = Random.Range(0f, 1f);
        //colliderr = GetComponent<Collider>();
        mask = LayerMask.GetMask("Ground Item");
        ground = LayerMask.GetMask("Ground");

        if (UnityEngine.Random.Range(0f, 1f) > spawnRate) { Destroy(transform.gameObject); }
        grounded = false;
    }

    void Update() {
        float currentPlayerDist = (player.transform.position - transform.position).sqrMagnitude;
        if (currentPlayerDist <= pickupDistance) {
            // Info Popup
            if (currentInfo == null) {
                currentInfo = Instantiate(infoPrefab, new Vector3(transform.position.x, startingZ + 2f, transform.position.z), Quaternion.identity);
                currentInfo.GetComponent<DisplayGroundItemInfo>().SetUp(item, amount);
            }

            // E to Pickup
            if (Input.GetKeyDown(KeyCode.E)) {
                if (PauseMenu.GameIsPaused) return;
                player.PickUpItem(this);
                if (currentInfo != null) { Destroy(currentInfo.gameObject); }
                Destroy(transform.gameObject);
                SFXManager.instance?.Play(sfx, 0.9f, 1.1f);
            }
        } else {
            if (currentInfo != null) { Destroy(currentInfo.gameObject); }
        }

        //lower to the ground
        if (!grounded){
            RaycastHit ray;
            Physics.Raycast(transform.position, Vector3.down, out ray, 1000f, ground);
            if (ray.distance > 2){
                startingZ -= Time.deltaTime;
                transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * 10, transform.position.z);
            }
            else
            grounded = true;
        }
    }

    void LateUpdate() {
        // Bounce Effect
        time += hoverRate * Time.deltaTime;

        //move away from other objects
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, stayAwayDist, mask);
        Vector2 totalForce = Vector2.zero;
        int i = 0;

        foreach(Collider col in hitColliders){
            if (col.transform.parent.gameObject.tag == "GroundItem" && col.transform.parent.gameObject != gameObject){
                totalForce += new Vector2(transform.position.x, transform.position.z) - new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.z);
                if (totalForce.sqrMagnitude == 0f) totalForce.x += 0.5f;
                i++;
            }
        }

        if (totalForce.x != 0) totalForce.x = 1f/totalForce.x * moveAwaySpeed * Time.deltaTime;
        if (totalForce.y != 0) totalForce.y = 1f/totalForce.y * moveAwaySpeed * Time.deltaTime;

        transform.position = new Vector3(transform.position.x + totalForce.x, startingZ + Mathf.Lerp(0, highestOffset, Mathf.Cos(time * Mathf.PI) * 0.5f + 0.5f), transform.position.z + totalForce.y);
    }

    
}