using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : UsableItem
{
    public override void Init() {
        // ToDo: implement
        IsInitted = true;
    }

    public override void Use() {
        if (!IsInitted) { Init(); }
        // ToDo: implement
    }
}
