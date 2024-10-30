using System;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI inventorySlotPrefab;
    [SerializeField] private Transform slotsParent;
    
    private ProductInventory productInventory;
    
    private void Awake()
    {
        inventorySlotPrefab.gameObject.SetActive(false);
    }

    private void Start()
    {
        productInventory = Player.Instance.HandleSystem.GetProductInventory();
        productInventory.OnItemAdded += ProductInventory_OnItemAdded;
        productInventory.OnItemRemoved += ProductInventory_OnItemRemoved;
        productInventory.OnSlotAmountChanged += ProductInventory_OnSlotAmountChanged;
        UpdateVisuals();
    }
    
    private void ProductInventory_OnSlotAmountChanged(object sender, EventArgs e)
    {
        UpdateVisuals();
    }
    
    private void ProductInventory_OnItemRemoved(object sender, EventArgs e)
    {
        UpdateVisuals();
    }
    
    private void ProductInventory_OnItemAdded(object sender, EventArgs e)
    {
        UpdateVisuals();
    }
    
    private void UpdateVisuals()
    {
        foreach (Transform child in slotsParent)
        {
            if (child == inventorySlotPrefab.transform) continue;
            Destroy(child.gameObject);
        }
        
        for(int i =0; i < productInventory.GetSlotAmount(); i++)
        {
            InventorySlotUI slot = Instantiate(inventorySlotPrefab, slotsParent);
            SlotInventoryItem<ProductSo> item = productInventory.GetSlot(i);
            slot.SetItem(item);
            slot.UpdateVisuals();
            slot.gameObject.SetActive(true);
        }
    }
}