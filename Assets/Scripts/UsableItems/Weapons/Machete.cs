using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machete : UsableItem
{
    [SerializeField] private float damage = 20f;
	[SerializeField] private float range = 8f;

    public override void Init() {
        // ToDo: implement
        IsInitted = true;
    }
    
    public override void Use() {
        if (!IsInitted) { Init(); }
        // ToDo: implement
    }
}
