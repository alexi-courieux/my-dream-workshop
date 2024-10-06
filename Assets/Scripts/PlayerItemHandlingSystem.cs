using System;
using System.Linq;
using UnityEngine;

public class PlayerItemHandlingSystem : MonoBehaviour, IHandleItems
{
    private const int BackpackSlots = 10;
    
    [SerializeField] private Transform itemSlot;
    [SerializeField] private GameObject backpackVisual;
    [SerializeField] private ProductDictionarySo backpackProductDictionarySo;
    
    private Item item;
    private bool isBackpackEquipped;
    private Item[] backpackItems;

    private void Start()
    {
        backpackItems = Array.Empty<Item>();
        backpackVisual.SetActive(false);
    }

    public void AddItem<T>(Item newItem) where T : Item
    {
        if (isBackpackEquipped && newItem is Product newProduct)
        {
            ProductSo productSo = newProduct.ProductSo;
            if (backpackItems.Length < BackpackSlots && backpackProductDictionarySo.products.Contains(productSo))
            {
                newProduct.DestroySelf();
                backpackItems = backpackItems.Append(newProduct).ToArray();
                return;
            }
        }
        
        item = newItem;
    }

    public Item[] GetItems<T>() where T : Item
    {
        return new[] {item};
    }

    public Item GetItem()
    {
        return item;
    }

    public void ClearItem(Item itemToClear)
    {
        item = null;
    }

    public bool HaveItems<T>() where T : Item
    {
        return item is T;
    }

    public bool HaveAnyItems()
    {
        return item is not null;
    }

    public Transform GetAvailableItemSlot<T>() where T : Item
    {
        return itemSlot;
    }

    public bool HasAvailableSlot<T>() where T : Item
    {
        return item is null;
    }
    
    public bool HaveBackpackItems()
    {
        return isBackpackEquipped && backpackItems.Length > 0;
    }
    
    public Item[] GetBackpackItems()
    {
        return backpackItems;
    }
    
    public void ClearItemFromBackpack(Item itemToClear)
    {
        backpackItems = backpackItems.Where(bItem => bItem != itemToClear).ToArray();
    }
    
    public void EquipBackpack()
    {
        isBackpackEquipped = true;
        backpackVisual.SetActive(true);
    }
    
    public void UnequipBackpack()
    {
        isBackpackEquipped = false;
        backpackVisual.SetActive(false);
    }
}