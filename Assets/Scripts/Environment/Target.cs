using UnityEngine;

// Source: https://www.youtube.com/watch?v=THnivyG0Mvo
public class Target : MonoBehaviour
{
    public float health = 50f;

	public void TakeDamage(float amount)
	{
		health -= amount;
		
		if (health < 1f)
			die();
	}

	void die()
	{
		Destroy(gameObject);
	}
}
