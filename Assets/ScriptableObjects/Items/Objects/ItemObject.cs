using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType
{
    Material,
    Consumable,
    Weapon,
    Deployable,
    Barricade
}

public class ItemObject : ScriptableObject
{
    public ItemType type;
    public string name;
    public Sprite uiDisplay;
    [TextArea(15,20)] public string description;
    public bool isMoveable = true;

    public GameObject groundPrefab;
    public GameObject usablePrefab;
}