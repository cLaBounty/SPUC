using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : UsableItem
{
    [SerializeField] private float healthIncrease = 10f;

    protected override void Init() {
        HideCrosshair();
    }

    protected override void Use() {
        player.GainHealth(healthIncrease);
        hotBar.HandleItemUse(itemObject);
        SFXManager.instance.Play("Eat", 0.95f, 1.05f);
    }
}
