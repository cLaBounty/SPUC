using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : UsableItem
{
	[SerializeField] private float healthIncreasePerSecond = 5f;
    [SerializeField] private float timeLimit = 20f;
    
    public override void Init() {
        // ToDo: implement
        IsInitted = true;
    }

    public override void Use() {
        if (!IsInitted) { Init(); }
        // ToDo: implement
    }
}
