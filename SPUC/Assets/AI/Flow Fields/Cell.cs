using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3 worldPos;
    public Vector2Int gridIndex;

    public Cell(Vector3 _worldPos, Vector2Int _gridIndex){
        worldPos = _worldPos;
        gridIndex = _gridIndex;
    }
}
