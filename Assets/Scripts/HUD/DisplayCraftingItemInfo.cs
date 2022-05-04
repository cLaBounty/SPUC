using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCraftingItemInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject recipeObject;
    [SerializeField] private GameObject ingredientPrefab;
    
    public void SetUp(ItemObject item, InventoryObject playerInventory) {
        nameText.text = item.name;
        descriptionText.text = item.description;

        foreach (Ingredient ingredient in item.recipe) {
            var obj = Instantiate(ingredientPrefab, Vector3.zero, Quaternion.identity, recipeObject.transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = ingredient.item.uiDisplay;
            obj.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.amount == 1 ? "" : ingredient.amount.ToString("n0");
            obj.transform.GetChild(3).gameObject.SetActive(!playerInventory.Has(ingredient.item, ingredient.amount)); // overlay if still needed
        }
    }
}
