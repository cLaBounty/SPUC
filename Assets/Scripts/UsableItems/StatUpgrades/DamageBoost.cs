using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoost : UsableItem
{
	[SerializeField] private float damageBoostValue = 0.2f;

    protected override void Init() {
        HideCrosshair();
    }

    protected override void Use() {
        player.damageMultiplier += damageBoostValue;
        hotBar.HandleItemUse(itemObject);
        SFXManager.instance.Play("Drink", 0.95f, 1.05f);
    }
}
