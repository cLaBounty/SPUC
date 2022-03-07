using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : UsableItem
{
	[SerializeField] private float healthIncrease = 15f;
    
    public override void Use() {
        GameObject.FindObjectOfType<Player>().GainHealth(healthIncrease);
    }
}
