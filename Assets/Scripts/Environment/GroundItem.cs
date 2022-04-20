using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;
    public int amount = 1;

    private float pickupDistance = 25f;
    public GameObject infoPrefab;
    private Player player;
    private GameObject currentInfo = null;

    // Bounce Effect
    private float hoverRate = 0.5f;
    private float highestOffset = 0.6f;
    private float startingZ;
    private float time = 0;

    //moving away from other objects
    [Header("Seperations")]
    public float stayAwayDist = 1f;
    public float moveAwaySpeed = 1f;
    Collider colliderr;

    void Start() {
        player = GameObject.FindObjectOfType<Player>();

        // Bounce Effect
        startingZ = transform.position.y;
        time = Random.Range(0f, 1f);
        colliderr = GetComponent<Collider>();
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
                Destroy(transform.gameObject);
            }
        } else {
            if (currentInfo != null) { Destroy(currentInfo.gameObject); }
        }

        colliderr.isTrigger = false;
    }

    void LateUpdate() {
        // Bounce Effect
        time += hoverRate * Time.deltaTime;
        //transform.position = new Vector3(transform.position.x, startingZ + Mathf.Lerp(0, highestOffset, Mathf.Cos(time * Mathf.PI) * 0.5f + 0.5f), transform.position.z);

        //move away from other objects
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, stayAwayDist);
        Vector2 totalForce = Vector2.zero;
        int i = 0;

        foreach(Collider col in hitColliders){
            if (col.gameObject.tag == "GroundItem" && col.gameObject != gameObject){
                totalForce += new Vector2(transform.position.x, transform.position.z) - new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.z);
                if (totalForce.sqrMagnitude == 0f) totalForce.x += 0.5f;
                i++;
            }
        }

        if (i > 1)
        Debug.Log(i);

        if (totalForce.x != 0) totalForce.x = 1f/totalForce.x * moveAwaySpeed * Time.deltaTime;
        if (totalForce.y != 0) totalForce.y = 1f/totalForce.y * moveAwaySpeed * Time.deltaTime;

        transform.position = new Vector3(transform.position.x + totalForce.x, startingZ + Mathf.Lerp(0, highestOffset, Mathf.Cos(time * Mathf.PI) * 0.5f + 0.5f), transform.position.z + totalForce.y);
        colliderr.isTrigger = true;
    }

    private void OnDestroy() {
        if (currentInfo != null) { Destroy(currentInfo.gameObject); }
    }
}