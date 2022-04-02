using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;
    public int amount = 1;

    [SerializeField] private float pickupDistance = 25f;
    public GameObject infoPrefab;
    private Player player;
    private GameObject currentInfo = null;

    // Bounce Effect
    private float hoverRate = 0.5f;
    private float highestOffset = 0.6f;
    private float startingZ;
    private float time = 0;

    void Start() {
        player = GameObject.FindObjectOfType<Player>();

        // Bounce Effect
        startingZ = transform.position.y;
        time = Random.Range(0f, 1f);
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
                player.PickUpItem(this);
                Destroy(transform.gameObject);
            }
        } else {
            if (currentInfo != null) { Destroy(currentInfo.gameObject); }
        }

        // Bounce Effect
        time += hoverRate * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, startingZ + Mathf.Lerp(0, highestOffset, Mathf.Cos(time * Mathf.PI) * 0.5f + 0.5f), transform.position.z);
    }

    private void OnDestroy() {
        if (currentInfo != null) { Destroy(currentInfo.gameObject); }
    }
}