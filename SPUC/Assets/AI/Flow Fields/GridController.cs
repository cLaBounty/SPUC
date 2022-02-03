using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;

    private void InitializeFlowField(){
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid();
    }

    void Update(){
        if(Input.GetMouseButton(0)) InitializeFlowField();

    }
}
