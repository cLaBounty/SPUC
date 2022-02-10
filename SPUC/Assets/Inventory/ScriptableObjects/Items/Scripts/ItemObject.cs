using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material,
    Consumable,
    Deployable,
    Barricade
}

public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;

    public string name;
    [TextArea(15,20)] public string description;
}
