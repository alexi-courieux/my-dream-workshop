using System;
using System.Linq;
using UnityEngine;

public class PlayerItemHandlingSystem : MonoBehaviour, IHandleItems
{
    public event EventHandler OnSlotSelected;
    
    private const int BackpackSlots = 7;
    private const int DefaultSlots = 3;
    
    [SerializeField] private Transform itemSlot;
    [SerializeField] private GameObject backpackVisual;
    [SerializeField] private ProductDictionarySo backpackProductDictionarySo;
    
    private Item[] items;
    private ItemInventory itemInventory;
    private int selectedSlotIndex;
    
    public int SelectedSlotIndex => selectedSlotIndex;

    private void Awake()
    {
        itemInventory = new ItemInventory(DefaultSlots, 1);
        items = new Item[DefaultSlots];
        selectedSlotIndex = 0;
    }

    private void Start()
    {
        backpackVisual.SetActive(false);
        InputManager.Instance.OnNextSlot += InputManager_OnNextSlot;
        InputManager.Instance.OnPreviousSlot += InputManager_OnPreviousSlot;
    }
    
    private void InputManager_OnNextSlot(object sender, EventArgs e)
    {
        selectedSlotIndex++;
        if (selectedSlotIndex >= items.Length) selectedSlotIndex = 0;
        RefreshItemVisuals();
        OnSlotSelected?.Invoke(this, EventArgs.Empty);
    }
    
    private void InputManager_OnPreviousSlot(object sender, EventArgs e)
    {
        selectedSlotIndex--;
        if (selectedSlotIndex < 0) selectedSlotIndex = items.Length - 1;
        RefreshItemVisuals();
        OnSlotSelected?.Invoke(this, EventArgs.Empty);
    }

    public void AddItem(Item newItem)
    {
        if (newItem is not Product product)
        {
            // TODO Player must be able to handle items, not only products
            Debug.LogError("Trying to add an item that is not a product");
            return;
        }
        if (!itemInventory.CanAddItem(product.ProductSo)) throw new Exception("Trying to add an item but no available slots");
        
        var selectedSlot = itemInventory.GetSlot(selectedSlotIndex);
        if (selectedSlot is null)
        {
            // Add item to selected slot
            itemInventory.AddItem(selectedSlotIndex, product.ProductSo, 1);
            items[selectedSlotIndex] = product;
            RefreshItemVisuals();
            return;
        }
        
        // Add item to first available slot
        itemInventory.TryAddItem(product.ProductSo, 1);
        int itemSlotIndex = itemInventory.GetItemSlot(product.ProductSo);
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
        itemInventory.RemoveItem(product!.ProductSo, 1, selectedSlotIndex);
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
        return itemInventory.GetItems().Count > 0;
    }
    
    public bool HaveAnyItemSelected()
    {
        return items[selectedSlotIndex] != null;
    }

    public bool HaveSpace(ItemSo itemToAdd)
    {
        return itemInventory.CanAddItem(itemToAdd);
    }

    public Transform GetAvailableItemSlot(Item newItem)
    {
        return itemSlot;
    }

    public bool HasAvailableSlot(Item newItem)
    {
        if (newItem is not Product product) return false;
        return itemInventory.CanAddItem(product.ProductSo);
    }
    
    public void EquipBackpack()
    {
        backpackVisual.SetActive(true);
        itemInventory.AddSlots(BackpackSlots);
        Array.Resize(ref items, items.Length + BackpackSlots);
    }
    
    public bool UnequipBackpack()
    {
        if (!itemInventory.TryRemoveSlots(BackpackSlots)) return false;
        backpackVisual.SetActive(false);
        for (int i =0; i < itemInventory.GetSlotAmount(); i++)
        {
            ItemSo itemSo = itemInventory.GetSlot(i)?.Item;
            if (itemSo is null) continue;
            bool found = false;
            for (int j = i; j < items.Length && !found; j++)
            {
                Item item = items[j];
                if (item is null) continue;
                if (item is not Product product)
                {
                    Debug.LogError("Inventory doesn't handle non-product items correctly yet!");
                    continue;
                }
                if (!product.ProductSo.Equals(itemSo)) continue;
                items[j] = null;
                items[i] = item;
                found = true;
            }
        }
        Array.Resize(ref items, items.Length - BackpackSlots);
        selectedSlotIndex = Mathf.Min(selectedSlotIndex, items.Length - 1);
        RefreshItemVisuals();
        OnSlotSelected?.Invoke(this, EventArgs.Empty);
        return true;
    }
    
    public ItemInventory GetProductInventory()
    {
        return itemInventory;
    }
    
    private void RefreshItemVisuals()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i]?.gameObject.SetActive(i == selectedSlotIndex);
        }
    }
}