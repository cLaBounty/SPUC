using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSwitching : MonoBehaviour
{
	private Animator animator;
	private string idleAnimation = "Idle";
	
	void Start() {
		animator = transform.gameObject.GetComponent<Animator>();
    }

    public void SwitchToItem(ItemObject item) {
        if (transform.childCount > 0) {
            foreach(Transform child in transform) {
                Destroy(child.gameObject);
            }
        }
        var inst = Instantiate(item.holdPrefab, transform, false);
        if (inst.transform.GetComponent<UsableItem>() == null) {
            GameObject.FindGameObjectWithTag("Crosshair").transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
		animator.Play(idleAnimation);
    }
}
