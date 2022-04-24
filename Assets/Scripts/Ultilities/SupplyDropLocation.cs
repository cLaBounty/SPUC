using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDropLocation : MonoBehaviour
{
    private const float RADIUS = 0.5f;

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, RADIUS);
    }
}
