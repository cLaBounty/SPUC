using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDebugger : MonoBehaviour
{
    public enum TerrainType{
        IMPASSABLE,
        ROUGH
    }

    public TerrainType type = TerrainType.IMPASSABLE;

    private void OnDrawGizmos() {
        switch(type){
            case TerrainType.IMPASSABLE:    Gizmos.color = Color.red; break;
            case TerrainType.ROUGH:         Gizmos.color = Color.yellow; break;
        }

        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }
}
