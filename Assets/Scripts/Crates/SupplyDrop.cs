using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDrop : MonoBehaviour
{
    [SerializeField] private float dropSpeed = 4f;
    [SerializeField] private GameObject thrusterOne;
    [SerializeField] private GameObject thrusterTwo;

    void Start() {
        GetComponent<ConstantForce>().force = new Vector3(0, 9.8f - dropSpeed, 0);
        transform.Rotate(0, Random.Range(0, 360), 0, Space.Self);
    }

    private void OnCollisionEnter(Collision other) {
        Destroy(thrusterOne);
        Destroy(thrusterTwo);
        transform.gameObject.GetComponent<Crate>().IsGrounded = true;
    }
}
