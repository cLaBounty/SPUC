using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Cell[,] grid {get; private set;}
    public Vector2Int gridSize {get; private set;}
    public float cellRadius {get; private set;}

    private int roughCost = 3;
    private float cellDiameter;
    public Cell destinationCell;

    public FlowField(float _cellRadius, Vector2Int _gridSize, int _roughCost = 3){
        cellRadius   = _cellRadius;
        cellDiameter = _cellRadius * 2f;
        gridSize     = _gridSize;
        roughCost    = _roughCost;
    }

    public void CreateGrid(){
        grid = new Cell[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; ++x){
            for(int y = 0; y < gridSize.y; ++y){
                Vector3 worldPos = new Vector3(cellDiameter * x + cellRadius, 0, cellDiameter * y + cellRadius);
                grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
            }
        }
    }

    public void CreateCostField(){
        Vector3 cellHalfExtents = Vector3.one * cellRadius;
        int terrainMask = LayerMask.GetMask("Impassible Terrain", "Rough Terrain");
        foreach (Cell currentCell in grid){
            Collider[] obstacles = Physics.OverlapBox(currentCell.worldPos, cellHalfExtents, Quaternion.identity, terrainMask);
            bool costIncreased = false;
            foreach (Collider collider in obstacles){
                if (collider.gameObject.layer == 8) {
                    currentCell.IncreaseCost(255);
                }
                else if (!costIncreased && collider.gameObject.layer == 9){
                    currentCell.IncreaseCost(roughCost);
                    costIncreased = true;
                }
            }
        }
    }

    public void CreateIntegrationField(Cell _destinationCell){
        destinationCell = _destinationCell;

        destinationCell.cost = 0;
        destinationCell.bestCost = 0;

        Queue<Cell> cellsToCheck = new Queue<Cell>();

        cellsToCheck.Enqueue(destinationCell);

        while(cellsToCheck.Count > 0){
            Cell curCell = cellsToCheck.Dequeue();
            List<Cell> currentNeighbors = GetNeighborCells(curCell.gridIndex);

            foreach(Cell neighbor in currentNeighbors){
                if (neighbor.cost == byte.MaxValue) continue;
                if (neighbor.cost + curCell.bestCost < neighbor.bestCost){
                    neighbor.bestCost = (ushort)(neighbor.cost + curCell.bestCost);
                    cellsToCheck.Enqueue(neighbor);
                }
            }
        }
    }

    public void CreateFlowField(){
        foreach (Cell cell in grid){
            List<Cell> neighborCells = GetNeighborCellsAll(cell.gridIndex);
            int bestCost = cell.bestCost;

            foreach(Cell neighbor in neighborCells){
                if (neighbor.bestCost < bestCost){
                    bestCost = neighbor.bestCost;
                    cell.bestDirection = neighbor.gridIndex - cell.gridIndex;
                }
            }
        }
    }

    private List<Cell> GetNeighborCells(Vector2Int nodeIndex){
        List<Cell> neighborCells = new List<Cell>();
        Cell newNeighbor;

        #region cardinals

        // RIGHT
        newNeighbor = GetNeighborCellSingular(nodeIndex, Vector2Int.right);
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        // UP
        newNeighbor = GetNeighborCellSingular(nodeIndex, Vector2Int.up);
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        // LEFT
        newNeighbor = GetNeighborCellSingular(nodeIndex, Vector2Int.left);
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        // DOWN
        newNeighbor = GetNeighborCellSingular(nodeIndex, Vector2Int.down);
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        #endregion

        return neighborCells;
    }

    private List<Cell> GetNeighborCellsAll(Vector2Int nodeIndex){
        List<Cell> neighborCells = new List<Cell>();
        Cell newNeighbor;

        #region cardinals

        // RIGHT
        newNeighbor = GetNeighborCellSingular(nodeIndex, Vector2Int.right);
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        // UP
        newNeighbor = GetNeighborCellSingular(nodeIndex, Vector2Int.up);
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        // LEFT
        newNeighbor = GetNeighborCellSingular(nodeIndex, Vector2Int.left);
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        // DOWN
        newNeighbor = GetNeighborCellSingular(nodeIndex, Vector2Int.down);
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        #endregion

        #region diagonals

        // UP RIGHT
        newNeighbor = GetNeighborCellSingular(nodeIndex, new Vector2Int(1,1));
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        // UP LEFT
        newNeighbor = GetNeighborCellSingular(nodeIndex, new Vector2Int(-1,1));
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        // DOWN RIGHT
        newNeighbor = GetNeighborCellSingular(nodeIndex, new Vector2Int(1,-1));
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        // DOWN LEFT
        newNeighbor = GetNeighborCellSingular(nodeIndex, new Vector2Int(-1,-1));
        if (newNeighbor != null) neighborCells.Add(newNeighbor);

        #endregion

        return neighborCells;
    }

    private Cell GetNeighborCellSingular(Vector2Int origin, Vector2Int direction){
        Vector2Int neighborPos = origin + direction;

        //error checking
        if (neighborPos.x < 0 || neighborPos.x >= gridSize.x || neighborPos.y < 0 || neighborPos.y >= gridSize.y)
            return null;
        return grid[neighborPos.x, neighborPos.y];
    }

    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        float percentX = worldPos.x / (gridSize.x * cellDiameter);
        float percentY = worldPos.z / (gridSize.y * cellDiameter);
 
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
 
        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        return grid[x, y];
    }
}
