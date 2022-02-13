using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;
    public Vector2 offset = new Vector2(0f, 0f);
    public GameObject target = null;
    public bool initialized = false;

    private void InitializeFlowField(){
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid();
    }

    void Update(){
        if (!initialized){
            target = GameObject.FindGameObjectWithTag("target");
            if (target != null){
                InitializeFlowField();
                curFlowField.CreateCostField();
                curFlowField.CreateIntegrationField(curFlowField.GetCellFromWorldPos(target.transform.position));
                curFlowField.CreateFlowField();

                initialized = true;
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        for(int x = 0; x < gridSize.x; ++x){
            for(int y = 0; y < gridSize.y; ++y){
                Vector3 worldPos = new Vector3(cellRadius * 2 * x + cellRadius, 0, cellRadius * 2 * y + cellRadius);
                Gizmos.DrawWireCube(worldPos, new Vector3(cellRadius * 2, 0.5f, cellRadius * 2));
            }
        }
    }
}
