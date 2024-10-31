using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Button sortCategoryButton;
    [SerializeField] private Button sortNameButton;
    [SerializeField] private Button sortAmountButton;
    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private Button eraseSearchButton;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private InventorySlotUI inventorySlotPrefab;
    
    private ItemInventory itemInventory;

    private void Awake()
    {
        sortCategoryButton.onClick.AddListener(() => itemInventory.SortItemsByTags());
        sortNameButton.onClick.AddListener(() => itemInventory.SortItemsByName());
        sortAmountButton.onClick.AddListener(() => itemInventory.SortItemsByAmount());
        searchInputField.onValueChanged.AddListener(Search);
        eraseSearchButton.onClick.AddListener(() => searchInputField.text = "");
    }

    private void Start()
    {
        inventorySlotPrefab.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Search(string search)
    {
        int[] indexes = itemInventory.Search(search);
        foreach (Transform child in slotsParent)
        {
            child.gameObject.SetActive(false);
        }
        foreach (int index in indexes)
        {
            slotsParent.GetChild(index).gameObject.SetActive(true);
        }
    }

    public void Show(ItemInventory inventory)
    {
        itemInventory = inventory;
        UpdateVisuals();
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateVisuals()
    {
        foreach (Transform child in slotsParent)
        {
            if (child == inventorySlotPrefab.transform) continue;
            Destroy(child.gameObject);
        }
        for (int i = 0; i < itemInventory.GetSlotAmount(); i++)
        {
            InventorySlotUI slot = Instantiate(inventorySlotPrefab, slotsParent);
            slot.gameObject.SetActive(true);
            var item = itemInventory.GetSlot(i);
            slot.SetItem(item);
            slot.UpdateVisuals(i);
        }
    }
}