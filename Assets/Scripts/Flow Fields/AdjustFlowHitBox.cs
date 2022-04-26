using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustFlowHitBox : MonoBehaviour
{
    bool set = false;
    Collider col = null;
    GridController grid = null;

    void Start()
    {
        col = GetComponent<Collider>();
        grid = GameObject.FindObjectOfType<GridController>();
    }

    void Update()
    {
        if (!set){
            if (grid != null){
                if (grid.initialized){
                    set = true;
                }
            }
        }
    }
}
