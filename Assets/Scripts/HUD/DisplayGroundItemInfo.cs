using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayGroundItemInfo : MonoBehaviour
{
    [SerializeField] private GameObject iconObject;
    [SerializeField] private TMP_Text nameText;
    
    public void SetUp(ItemObject item, int amount) {
        iconObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite = item.uiDisplay;
        iconObject.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
        iconObject.GetComponentInChildren<TextMeshProUGUI>().text = amount == 1 ? "" : amount.ToString("n0");
        nameText.text = item.name;
    }
}
