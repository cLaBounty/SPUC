using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoost : UsableItem
{
	[SerializeField] private string useAnimation = "Consume";
	[SerializeField] private float defenseBoostValue = 1f;
    [SerializeField] private float useTime = 0.5f;

	private Animator animator;
    private float coolDownTime;

    protected override void Init() {
		animator = GameObject.FindObjectOfType<ItemSwitching>().transform.gameObject.GetComponent<Animator>();
		coolDownTime = useTime;
        HideCrosshair();
    }

	private void Update() {
		base.Update();
		coolDownTime += Time.deltaTime;
	}

    protected override void Use() {
		if (coolDownTime >= useTime) { coolDownTime = 0; }
		else { return; }
		animator.Play(useAnimation);

        player.defense += defenseBoostValue;
        SFXManager.instance.Play("Drink", 0.95f, 1.05f);
		StartCoroutine(UseTimer());  
    }

	IEnumerator UseTimer() {
		yield return new WaitForSeconds(useTime);
        hotBar.HandleItemUse(itemObject);
	}
}
