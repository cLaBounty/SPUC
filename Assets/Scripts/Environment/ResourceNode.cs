using UnityEngine;
using System;

public class ResourceNode : MonoBehaviour
{
	[SerializeField] float health = 100f;
	[SerializeField] int resources = 10;
	[Range(0,1)][SerializeField] private float spawnRate = 1f;

    public ItemObject item;
	public ParticleSystem harvestEffect;

	private bool particleTriggered = false;

	private void Start() {
		if (UnityEngine.Random.Range(0f, 1f) > spawnRate) { Destroy(transform.gameObject); }
	}

	void Update()
	{
		if (particleTriggered && !harvestEffect.isPlaying)
			Destroy(gameObject);
	}

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
			SFXManager.instance.Play("Rock Break", 0.9f, 1.1f);

			foreach(Transform child in transform)
			{
				if (child.name != harvestEffect.name)
					Destroy(child.gameObject);
			}
			harvestEffect.Play();
			particleTriggered = true;
			GetComponent<Collider>().enabled = false;
		}
		else{
			SFXManager.instance.Play("Rock Crack", 0.9f, 1.1f);
		}

		resources -= amount;
		return amount;
	}
}