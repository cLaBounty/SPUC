using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : UsableItem
{
    [SerializeField] private float damage = 15f;
	[SerializeField] private float range = 3f;

    public override void Init() {
        // ToDo: implement
        IsInitted = true;
    }

    public override void Use() {
        if (!IsInitted) { Init(); }
        // ToDo: implement
    }
}
