using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventoryItemInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    
    public void SetUp(ItemObject item) {
        nameText.text = item.name;
    }
}
