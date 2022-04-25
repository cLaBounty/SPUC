using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoost : UsableItem
{
	private const float MaxHealthMultiplier = 3f;
    
    private Player player;
    private HotBar hotBar;

    protected override void Init() {
        player = GameObject.FindObjectOfType<Player>();
        hotBar = GameObject.FindObjectOfType<HotBar>();
        HideCrosshair();
    }

    protected override void Use() {
        // ToDo: implement
        
        hotBar.HandleItemUse(itemObject);
        SFXManager.instance.Play("Eat", 0.95f, 1.05f); // ToDo: find powerup sound effect
    }
}
