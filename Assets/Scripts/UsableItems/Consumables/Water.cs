using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : UsableItem
{
	[SerializeField] private float healthIncrease = 15f;
    
    private Player player;

    public override void Init() {
        player = GameObject.FindObjectOfType<Player>();
        IsInitted = true;
    }

    public override void Use() {
        if (!IsInitted) { Init(); }
        player.GainHealth(healthIncrease);
    }
}
