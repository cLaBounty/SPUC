using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoost : UsableItem
{
	[SerializeField] private float defenseBoostValue = 1f;

    protected override void Init() {
        HideCrosshair();
    }

    protected override void Use() {
        player.defense += defenseBoostValue;
        hotBar.HandleItemUse(itemObject);
        SFXManager.instance.Play("Drink", 0.95f, 1.05f);
    }
}
