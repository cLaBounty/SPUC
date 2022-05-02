using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CrateRarity {
    Common,
    Rare,
    Legendary
}

public class Crate : MonoBehaviour
{
    [SerializeField] private CrateRarity rarity;
    [Range(0,1)][SerializeField] private float spawnRate = 1f;
    [SerializeField] private float openRange = 5f;

    public GameObject infoPrefab;
    private GameObject currentInfo = null;

    private Player player;
    private HotBar hotBar;
    private ItemObject key;

    private float openRangeSqr;
    public bool IsGrounded = true;

    private void Start() {
        player = GameObject.FindObjectOfType<Player>();
        hotBar = GameObject.FindObjectOfType<HotBar>();
        SetRarity(rarity);
        openRangeSqr = openRange * openRange;
        if (UnityEngine.Random.Range(0f, 1f) > spawnRate) { Destroy(transform.gameObject); }
    }

    private void Update() {
        if (player == null) return;

        float currentPlayerDist = (player.transform.position - transform.position).sqrMagnitude;
        if (currentPlayerDist <= openRangeSqr) {
            // Info Popup
            if (currentInfo == null && IsGrounded) {
                currentInfo = Instantiate(infoPrefab, new Vector3(transform.position.x, 0 + 3f, transform.position.z), Quaternion.identity);
                currentInfo.GetComponent<DisplayCrateInfo>().SetUp(rarity);
            }

            // E to Open
            if (Input.GetKeyDown(KeyCode.E) && player.inventory.Has(key, 1)) {
                if (PauseMenu.GameIsPaused) return;
                SpawnItems();
                hotBar.HandleItemUse(key);
                if (currentInfo != null) { Destroy(currentInfo.gameObject); }
                Destroy(transform.gameObject);
            }
        } else {
            if (currentInfo != null) { Destroy(currentInfo.gameObject); }
        }
    }

    public void SetRarity(CrateRarity rarity) {
        this.rarity = rarity;
        SetMaterial();
        SetKey();
    }

    private void SetMaterial() {
        Material material = (Material)Resources.Load("M_" + rarity.ToString() + "Crate", typeof(Material));
        foreach(Transform child in transform) {
            child.GetComponent<MeshRenderer>().material = material;
        }
    }

    private void SetKey() {
        key = Resources.Load<ItemObject>("Items/Key/" + rarity.ToString() + "Key");
    }

    private void SpawnItems() {
        SFXManager.instance.Play("Unlock", 0.9f, 1.1f);

        List<InventorySlot> items = new List<InventorySlot>();
        switch (rarity) {
            case CrateRarity.Common:
                items = GetCommonItems();
                break;
            case CrateRarity.Rare:
                items = GetRareItems();
                break;
            case CrateRarity.Legendary:
                items = GetLegendaryItems();
                break;
        }

        foreach (InventorySlot slot in items) {
            var inst = Instantiate(slot.item.groundPrefab);
            inst.GetComponent<GroundItem>().amount = slot.amount;
            
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 0.5f; // Fixes issue of items spawning underground
            inst.transform.position = spawnPosition;
        }
    }

    private List<InventorySlot> GetCommonItems() {
        List<InventorySlot> result = new List<InventorySlot>();
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.Ammo), 10));
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.Material), 10));
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.Consumable), 1));
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.MeleeWeapon), 1));
        return result;
    }

    private List<InventorySlot> GetRareItems() {
        List<InventorySlot> result = new List<InventorySlot>();
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.Ammo), 20));
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.Material), 20));
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.Consumable), 3));
        result.Add(new InventorySlot(GetRandomItemOfTwoTypes(ItemType.Deployable, ItemType.MeleeWeapon), 1));
        return result;
    }

    private List<InventorySlot> GetLegendaryItems() {
        List<InventorySlot> result = new List<InventorySlot>();
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.Ammo), 40));
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.Material), 40));
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.Consumable), 5));
        result.Add(new InventorySlot(GetRandomItemOfType(ItemType.FirearmWeapon), 1));
        result.Add(new InventorySlot(GetRandomItemOfTwoTypes(ItemType.Deployable, ItemType.StatUpgrade), 1));
        return result;
    }

    private ItemObject GetRandomItemOfType(ItemType type) {
        ItemObject[] items = Resources.LoadAll<ItemObject>("Items/" + type.ToString());
        return items[UnityEngine.Random.Range(0, items.Length)];
    }

    private ItemObject GetRandomItemOfTwoTypes(ItemType type1, ItemType type2) {
        ItemObject[] items1 = Resources.LoadAll<ItemObject>("Items/" + type1.ToString());
        ItemObject[] items2 = Resources.LoadAll<ItemObject>("Items/" + type2.ToString());

        ItemObject[] mixedItems = new ItemObject[items1.Length + items2.Length];
        Array.Copy(items1, mixedItems, items1.Length);
        Array.Copy(items2, 0, mixedItems, items1.Length, items2.Length);

        return mixedItems[UnityEngine.Random.Range(0, mixedItems.Length)];
    }
}
