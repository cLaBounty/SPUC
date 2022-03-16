using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem : MonoBehaviour
{
    protected bool IsInitted = false;
    public abstract void Init();
    public abstract void Use();
}
