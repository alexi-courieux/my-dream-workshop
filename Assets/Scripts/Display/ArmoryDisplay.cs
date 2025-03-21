using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class ArmoryDisplay : MonoBehaviour, IInteractable, IHandleItems
{
    [SerializeField] private Transform helmetSlot;
    [SerializeField] private Transform chestSlot;
    [SerializeField] private Transform pantsSlot;
    [SerializeField] private Transform bootSlot;
    [SerializeField] private ItemTypeSo helmetType;
    [SerializeField] private ItemTypeSo chestType;
    [SerializeField] private ItemTypeSo pantsType;
    [SerializeField] private ItemTypeSo bootType;
    private List<Product> _items;
    private List<ItemTypeSo> _itemTypes;
    private ItemTypeSo[] _allowedProductTypes;

    private void Awake()
    {
        _items = new List<Product>();
        _itemTypes = new List<ItemTypeSo>();
        _allowedProductTypes = new []{helmetType, chestType, pantsType, bootType};
    }

    public void Interact()
    {
        if (Player.Instance.HandleSystem.HaveAnyItems())
        {
            Item playerItem = Player.Instance.HandleSystem.GetSelectedItem();
            if (playerItem is not Product product) return;
            if (!HasAvailableSlot(playerItem)) return;

            product.SetParent(this);
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
        if (newItem is not Product product)
        {
            return;
        }
        _items.Add(product);
        _itemTypes.AddRange(product.ProductSo.types);
    }

    public Item[] GetItems<T>() where T : Item
    {
        if (typeof(T) != typeof(Product)) return null;

        return _items.Cast<Item>().ToArray();
    }

    public void ClearItem(Item itemToClear)
    {
        if (itemToClear is not Product product) throw new Exception("This station can only hold products!");
        _items.Remove(product);
        foreach (var type in product.ProductSo.types)
        {
            _itemTypes.Remove(type);
        }
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
        if (item is not Product product)
        {
            throw new Exception("This station can only hold products!, use HasAvailableSlot before calling this method");
        }
        
        if(product.ProductSo.types.Contains(helmetType))
        {
            return helmetSlot;
        }
        if(product.ProductSo.types.Contains(chestType))
        {
            return chestSlot;
        }
        if(product.ProductSo.types.Contains(pantsType))
        {
            return pantsSlot;
        }
        if(product.ProductSo.types.Contains(bootType))
        {
            return bootSlot;
        }
        return null;
    }

    public bool HasAvailableSlot(Item item)
    {
        if (item is not Product product) return false;
        if (_allowedProductTypes.Length > 0
            && !product.ProductSo.types.Any(type => _allowedProductTypes.Contains(type))) return false;
        // NOTE: We assume no product can have more than one of the allowed types (i.e. helmet & chest)
        return !product.ProductSo.types.Any(type => _itemTypes.Contains(type));
    }
}
