using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : UsableItem
{
	[SerializeField] private float healthIncrease = 10f;

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
