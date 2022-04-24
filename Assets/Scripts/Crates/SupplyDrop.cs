using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDrop : MonoBehaviour
{
    private const float DROP_SPEED = 4f;

    public GameObject thrusterOne;
    public GameObject thrusterTwo;

    void Start() {
        GetComponent<ConstantForce>().force = new Vector3(0, 9.8f - DROP_SPEED, 0);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Ground") {
            Destroy(thrusterOne);
            Destroy(thrusterTwo);
        }
    }
}
