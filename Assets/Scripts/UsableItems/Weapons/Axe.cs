using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : UsableItem
{
    [SerializeField] private float damage = 10f;
	[SerializeField] private float range = 5f;

    public override void Init() {
        // ToDo: implement
        IsInitted = true;
    }

    public override void Use() {
        if (!IsInitted) { Init(); }
        // ToDo: implement
    }
}
