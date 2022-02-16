using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustFlowHitBox : MonoBehaviour
{
    bool set = false;
    Collider col = null;
    GridController grid = null;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        grid = GameObject.FindObjectOfType<GridController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!set){
            if (grid != null){
                if (grid.initialized){
                   // col

                    set = true;
                }
            }
        }
    }
}
