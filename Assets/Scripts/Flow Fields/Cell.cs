using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3 worldPos;
    public Vector2Int gridIndex;
    public Vector2Int bestDirection;
    public ushort bestCost;
    public byte cost; 
    public byte baseCost;
    
    public Cell(Vector3 _worldPos, Vector2Int _gridIndex){
        worldPos = _worldPos;
        gridIndex = _gridIndex;
        cost = 1;
        baseCost = 0;
        bestCost = ushort.MaxValue;
        bestDirection = Vector2Int.right;
    }

    public void IncreaseCost(int amount){
        if (cost == byte.MaxValue) return;
        if (amount + cost >= 255) cost = byte.MaxValue;
        else cost += (byte)amount;
    }
}
