using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponDisplay : MonoBehaviour, IInteractable, IHandleItems
{
    [SerializeField] private Transform[] itemSlots;
    [SerializeField] private ItemTypeSo[] allowedProductTypes;
    private List<Product> _items;
    private int _capacity;

    private void Awake()
    {
        _items = new List<Product>();
        _capacity = itemSlots.Length;
    }
    public void Interact()
    {
        if (Player.Instance.HandleSystem.HaveAnyItems())
        {
            Item playerItem = Player.Instance.HandleSystem.GetSelectedItem();
            if (playerItem is not Product p) return;
            if (!HasAvailableSlot(playerItem)) return;
            
            p.SetParent(this);
        }
        else
        {
            if (_items.Count <= 0) return;
            
            Item item = _items[0];
            item.SetParent(Player.Instance.HandleSystem);
        }
    }
        public void AddItem(Item newItem)
    {
        if (newItem is not Product p)
        {
            throw new Exception("This station can only hold products!");
        }
        _items.Add(p);
    }

    public Item[] GetItems<T>() where T : Item
    {
        if (typeof(T) != typeof(Product)) return null;
        
        return _items.Cast<Item>().ToArray();
    }

    public void ClearItem(Item itemToClear)
    {
        if (itemToClear is not Product p) throw new Exception("This station can only hold products!");
        _items.Remove(p);
    }

    public bool HaveItems<T>() where T : Item
    {
        if (typeof(T) != typeof(Product))
        {
            Debug.LogWarning("This station can only hold products!");
            return false;
        }
    
        return _items.Count > 0;
    }

    public bool HaveAnyItems()
    {
        return _items.Count > 0;
    }

    public Transform GetAvailableItemSlot(Item item)
    {
        foreach (Transform itemSlot in itemSlots)
        {
            if (itemSlot.childCount <= 0) return itemSlot;
        }
        throw new Exception("No available slot found!");
    }

    public bool HasAvailableSlot(Item item)
    {
        if (item is not Product p) return false;
        if (allowedProductTypes.Length > 0
            && !allowedProductTypes.Any(t => p.ProductSo.types.Any(pt => pt.IsType(t)))) return false;
        return _items.Count < _capacity;
    }

}
