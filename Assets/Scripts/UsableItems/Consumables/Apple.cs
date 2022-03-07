using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : UsableItem
{
	[SerializeField] private float healthIncrease = 10f;
    
    public override void Use() {
        GameObject.FindObjectOfType<Player>().GainHealth(healthIncrease);
    }
}
