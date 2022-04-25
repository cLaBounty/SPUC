using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : UsableItem
{
    [SerializeField] private float healthIncrease = 15f;

    protected override void Init() {
        HideCrosshair();
    }

    protected override void Use() {
        player.GainHealth(healthIncrease);
        hotBar.HandleItemUse(itemObject);
        SFXManager.instance.Play("Drink", 0.95f, 1.05f);
    }
}
