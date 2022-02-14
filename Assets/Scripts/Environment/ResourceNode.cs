using UnityEngine;
using System;

public class ResourceNode : MonoBehaviour
{
    public ItemObject item;
	[SerializeField] float health = 50f;
	[SerializeField] int resources = 10;
	
	

	public int harvest(float damage)
	{
		int amount;
		// resources per health
		float RPH = resources / health;
		health -= damage;
		amount = (int)Math.Round(RPH * damage);

		if (resources < amount)
		{
			amount = resources;
		}

		if (health < 1f)
		{
			Destroy(gameObject);
		}
		Debug.Log(amount);
		resources -= amount;
		return amount;
	}
}