using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material,
    Consumable,
    Weapon,
    Deployable,
    Barricade
}

public abstract class ItemObject : ScriptableObject
{
    public int id;
    public Sprite uiDisplay;
    public ItemType type;
    public string name;
    [TextArea(15,20)] public string description;
}

[System.Serializable]
public class Item
{
    // TODO: remove this layer of abstraction
    public ItemObject itemObject;

    public int id;
    public string name;
    public Sprite uiDisplay;

    public Item(ItemObject item) {
        itemObject = item;

        id = item.id;
        name = item.name;
        uiDisplay = item.uiDisplay;
    }
}