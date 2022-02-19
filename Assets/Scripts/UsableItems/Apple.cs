using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : UsableItem
{
	[SerializeField] private float healthIncrease = 10f;

    private Player player;

	private void Start() {
		player = GameObject.FindObjectOfType<Player>();
	}
    
    public override void Use() {
        player.GainHealth(healthIncrease);
    }
}
