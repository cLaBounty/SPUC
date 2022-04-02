using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem : MonoBehaviour
{
    public ItemObject itemObject;
    public abstract void Init();
    public abstract void Use();
    private void Start() { Init(); }
}
