using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class Item : ScriptableObject
{
    public int test = 0;
}

public class Consumable : Item
{

}