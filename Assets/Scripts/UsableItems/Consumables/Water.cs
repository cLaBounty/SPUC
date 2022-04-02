using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : UsableItem
{
	private const float HEALTH_INCREASE = 15f;
    
    private Player player;
    private HotBar hotBar;

    public override void Init() {
        player = GameObject.FindObjectOfType<Player>();
        hotBar = GameObject.FindObjectOfType<HotBar>();
    }

    public override void Use() {
        if (hotBar == null) { Init(); }
        player.GainHealth(HEALTH_INCREASE);
        hotBar.HandleItemUse(itemObject);
    }
}
