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
    
    private ProductInventory productInventory;

    private void Awake()
    {
        sortCategoryButton.onClick.AddListener(() => productInventory.SortItemsByTags());
        sortNameButton.onClick.AddListener(() => productInventory.SortItemsByName());
        sortAmountButton.onClick.AddListener(() => productInventory.SortItemsByAmount());
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
        int[] indexes = productInventory.Search(search);
        foreach (Transform child in slotsParent)
        {
            child.gameObject.SetActive(false);
        }
        foreach (int index in indexes)
        {
            slotsParent.GetChild(index).gameObject.SetActive(true);
        }
    }

    public void Show(ProductInventory inventory)
    {
        productInventory = inventory;
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
        for (int i = 0; i < productInventory.GetSlotAmount(); i++)
        {
            InventorySlotUI slot = Instantiate(inventorySlotPrefab, slotsParent);
            slot.gameObject.SetActive(true);
            var item = productInventory.GetSlot(i);
            slot.SetItem(item);
            slot.UpdateVisuals(i);
        }
    }
}