using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : UsableItem
{
	private const float HEALTH_INCREASE = 10f;

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
