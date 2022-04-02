using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType
{
    Ammo,
    ResourceCollector,
    Material,
    Consumable,
    Deployable,
    Barricade,
    MeleeWeapon,
    FirearmWeapon,
    StatUpgrade
}

[CreateAssetMenu(fileName = "NewItemObject", menuName = "InventorySystem/Item")]
public class ItemObject : ScriptableObject
{
    public ItemType type;
    public string name;
    [TextArea(5,10)] public string description;
    public Sprite uiDisplay;
    public bool isMoveable = true;
    public Ingredient[] recipe;
    public GameObject groundPrefab;
    public GameObject holdPrefab;
}

[Serializable]
public struct Ingredient {
    public ItemObject item;
    public int amount;
}