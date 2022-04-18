using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCrateInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    
    public void SetUp(CrateRarity rarity) {
        nameText.text = rarity.ToString() + " Crate";
    }
}
