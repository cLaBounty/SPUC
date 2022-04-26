using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType
{
    ResourceCollector,
    Ammo,
    Material,
    Consumable,
    Deployable,
    MeleeWeapon,
    FirearmWeapon,
    StatUpgrade,
    Key
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
    public int craftAmount = 1;
    public GameObject groundPrefab;
    public GameObject holdPrefab;
}

[Serializable]
public struct Ingredient {
    public ItemObject item;
    public int amount;
}