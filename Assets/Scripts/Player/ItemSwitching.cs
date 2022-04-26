using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSwitching : MonoBehaviour
{
    [SerializeField] private string idleAnimation = "Idle";

	private Animator animator;
	
	void Start() {
		animator = transform.gameObject.GetComponent<Animator>();
    }

    public void SwitchToItem(ItemObject item) {
        if (transform.childCount > 0) {
            foreach(Transform child in transform) {
                // Destroy Preview if Deployable Item
                BaseDeployable baseDeployable = child.gameObject.GetComponent<BaseDeployable>();
                if (baseDeployable?.deployedPreview != null) { Destroy(baseDeployable.deployedPreview); }

                Destroy(child.gameObject);
            }
        }

        var inst = Instantiate(item.holdPrefab, transform, false);
        if (inst.transform.GetComponent<UsableItem>() == null) {
            GameObject.FindGameObjectWithTag("Crosshair").transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
		animator?.Play(idleAnimation);
    }
}
