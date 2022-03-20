using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : UsableItem
{
	private const float HEALTH_INCREASE = 15f;
    
    private Player player;

    public override void Init() {
        player = GameObject.FindObjectOfType<Player>();
        IsInitted = true;
    }

    public override void Use() {
        Init();
        player.GainHealth(HEALTH_INCREASE);
    }
}
