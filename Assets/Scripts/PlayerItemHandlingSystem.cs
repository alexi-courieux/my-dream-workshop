using System;
using System.Linq;
using UnityEngine;

public class PlayerItemHandlingSystem : MonoBehaviour, IHandleItems
{
    private const int BackpackSlots = 8;
    private const int DefaultSlots = 2;
    
    [SerializeField] private Transform itemSlot;
    [SerializeField] private GameObject backpackVisual;
    [SerializeField] private ProductDictionarySo backpackProductDictionarySo;
    
    private Item[] items;
    private ProductInventory productInventory;
    private int selectedSlotIndex;

    private void Awake()
    {
        productInventory = new ProductInventory(DefaultSlots, 1);
        items = new Item[DefaultSlots];
        selectedSlotIndex = 0;
    }

    private void Start()
    {
        backpackVisual.SetActive(false);
    }

    public void AddItem(Item newItem)
    {
        if (newItem is not Product product)
        {
            // TODO Player must be able to handle items, not only products
            Debug.LogError("Trying to add an item that is not a product");
            return;
        }
        if (!productInventory.CanAddItem(product.ProductSo)) throw new Exception("Trying to add an item but no available slots");
        
        var selectedSlot = productInventory.GetSlot(selectedSlotIndex);
        if (selectedSlot is null)
        {
            // Add item to selected slot
            productInventory.AddItem(selectedSlotIndex, product.ProductSo, 1);
            items[selectedSlotIndex] = product;
            RefreshItemVisuals();
            return;
        }
        
        // Add item to first available slot
        productInventory.TryAddItem(product.ProductSo, 1);
        int itemSlotIndex = productInventory.GetItemSlot(product.ProductSo);
        items[itemSlotIndex] = product;
        RefreshItemVisuals();
    }

    public Item[] GetItems<T>() where T : Item
    {
        return items.Where(item => item is T).ToArray();
    }

    public Item GetSelectedItem()
    {
        return items[selectedSlotIndex];
    }
    
    public T GetItem<T>() where T : Item
    {
        return items[selectedSlotIndex] as T;
    }

    public void ClearItem(Item itemToClear)
    {
        Product product = itemToClear as Product;
        items[selectedSlotIndex] = null;
        productInventory.RemoveItem(product!.ProductSo, 1);
        RefreshItemVisuals();
    }

    public bool HaveItems<T>() where T : Item
    {
        return items.Any(item => item is T);
    }
    
    public bool HaveItemSelected<T>() where T : Item
    {
        return items[selectedSlotIndex] is T;
    }

    public bool HaveAnyItems()
    {
        return productInventory.GetItems().Count > 0;
    }
    
    public bool HaveAnyItemSelected()
    {
        return items[selectedSlotIndex] != null;
    }

    public Transform GetAvailableItemSlot(Item newItem)
    {
        return itemSlot;
    }

    public bool HasAvailableSlot(Item newItem)
    {
        if (newItem is not Product product) return false;
        return productInventory.CanAddItem(product.ProductSo);
    }
    
    public bool HaveBackpackItems()
    {
        return false; // TODO Remove
    }
    
    public Item[] GetBackpackItems()
    {
        return null; // TODO Remove
    }
    
    public void ClearItemFromBackpack(Item itemToClear)
    {
        return; // TODO Remove
    }
    
    public void EquipBackpack()
    {
        backpackVisual.SetActive(true);
        productInventory.AddSlots(BackpackSlots);
    }
    
    public void UnequipBackpack()
    {
        backpackVisual.SetActive(false);
    }
    
    public ProductInventory GetProductInventory()
    {
        return productInventory;
    }
    
    private void RefreshItemVisuals()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i]?.gameObject.SetActive(i == selectedSlotIndex);
        }
    }
}