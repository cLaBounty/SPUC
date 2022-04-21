using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : UsableItem
{
	private const float HEALTH_INCREASE = 15f;
    
    private Player player;
    private HotBar hotBar;

    protected override void Init() {
        player = GameObject.FindObjectOfType<Player>();
        hotBar = GameObject.FindObjectOfType<HotBar>();
        HideCrosshair();
    }

    protected override void Use() {
        player.GainHealth(HEALTH_INCREASE);
        hotBar.HandleItemUse(itemObject);
        SFXManager.instance.Play("Drink", 0.95f, 1.05f);
    }
}
