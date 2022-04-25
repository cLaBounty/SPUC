using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : UsableItem
{
	private string useAnimation = "Consume";

	private const float HEALTH_INCREASE = 10f;
	private const float USE_TIME = .5f;

	private float coolDownTime;

    private Player player;
    private HotBar hotBar;
	private Animator animator;

    protected override void Init() {
        player = GameObject.FindObjectOfType<Player>();
        hotBar = GameObject.FindObjectOfType<HotBar>();
		animator = GameObject.FindObjectOfType<ItemSwitching>().transform.gameObject.GetComponent<Animator>();
		coolDownTime = USE_TIME;
        HideCrosshair();
    }

	private void Update() {
		base.Update();
		coolDownTime += Time.deltaTime;
	}

    protected override void Use() {
		if (coolDownTime >= USE_TIME) { coolDownTime = 0; }
		else { return; }
		animator.Play(useAnimation);
		SFXManager.instance.Play("Eat", 0.95f, 1.05f);
        StartCoroutine(UseTimer()); 
    }

	IEnumerator UseTimer() {
		yield return new WaitForSeconds(USE_TIME);
		player.GainHealth(HEALTH_INCREASE);
        hotBar.HandleItemUse(itemObject);
	}
}
