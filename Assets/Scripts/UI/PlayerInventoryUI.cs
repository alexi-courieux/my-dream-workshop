using System;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI inventorySlotPrefab;
    [SerializeField] private Transform slotsParent;
    
    private ItemInventory itemInventory;
    
    private void Awake()
    {
        inventorySlotPrefab.gameObject.SetActive(false);
    }

    private void Start()
    {
        itemInventory = Player.Instance.HandleSystem.GetProductInventory();
        itemInventory.OnItemAdded += ItemInventory_OnItemAdded;
        itemInventory.OnItemRemoved += ItemInventory_OnItemRemoved;
        itemInventory.OnSlotAmountChanged += ItemInventory_OnSlotAmountChanged;
        
        Player.Instance.HandleSystem.OnSlotSelected += PlayerHandleSystem_OnSlotSelected;
        UpdateVisuals();
    }
    
    private void PlayerHandleSystem_OnSlotSelected(object sender, EventArgs e)
    {
        UpdateVisuals();
    }
    
    private void ItemInventory_OnSlotAmountChanged(object sender, EventArgs e)
    {
        UpdateVisuals();
    }
    
    private void ItemInventory_OnItemRemoved(object sender, EventArgs e)
    {
        UpdateVisuals();
    }
    
    private void ItemInventory_OnItemAdded(object sender, EventArgs e)
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
        
        for(int i =0; i < itemInventory.GetSlotAmount(); i++)
        {
            InventorySlotUI slot = Instantiate(inventorySlotPrefab, slotsParent);
            var item = itemInventory.GetSlot(i);
            slot.SetItem(item);
            slot.UpdateVisuals(i);
            slot.gameObject.SetActive(true);
        }
    }
}