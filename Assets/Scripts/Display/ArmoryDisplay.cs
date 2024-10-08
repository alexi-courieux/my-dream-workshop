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
    private List<FinalProduct> _items;
    private FinalProductSo.ItemType TypeFinalProductSo;
    private bool haveHelmet = false;
    private bool haveChest = false;
    private bool havePants = false;
    private bool haveBoot = false;

    private void Awake()
        {
            _items = new List<FinalProduct>();
        }

    public void Interact()
    {
        if (Player.Instance.HandleSystem.HaveAnyItems())
            {
                if (!HasAvailableSlot<FinalProduct>()) return;
            
                if (Player.Instance.HandleSystem.GetItem() is not FinalProduct product)
                {
                    Debug.LogWarning("Station can only hold products!");
                    return;
                }         
                TypeFinalProductSo = product.FinalProductSo.itemType;
                product.SetParent<FinalProduct>(this);
            }
            else
            {
                if (_items.Count <= 0) return;
            
                Item item = _items[0];
                item.SetParent<Item>(Player.Instance.HandleSystem);
            }
    }
        public void AddItem<T>(Item newItem) where T : Item
    {
        if (typeof(T) != typeof(FinalProduct))
        {
            Debug.LogWarning("This station can only hold final products!");
            return;
        }
        _items.Add(newItem as FinalProduct);
    }

    public Item[] GetItems<T>() where T : Item
    {
        if (typeof(T) != typeof(FinalProduct))
        {
            Debug.LogWarning("This station can only hold final products!");
            return null;
        }
    
        return _items.Cast<Item>().ToArray();
    }

    public void ClearItem(Item itemToClear)
    {
        _items.Remove(itemToClear as FinalProduct);
    }

    public bool HaveItems<T>() where T : Item
    {
        if (typeof(T) != typeof(FinalProduct))
        {
            Debug.LogWarning("This station can only hold final products!");
            return false;
        }
    
        return _items.Count > 0;
    }

    public bool HaveAnyItems()
    {
        return _items.Count > 0;
    }

    public Transform GetAvailableItemSlot<T>() where T : Item
    {
        switch (TypeFinalProductSo)
        {
            case FinalProductSo.ItemType.Helmet:
                haveHelmet = false;
                return helmetSlot;
            case FinalProductSo.ItemType.Chest:
                haveChest = false;
                return chestSlot;
            case FinalProductSo.ItemType.Pants:
                havePants = false;
                return pantsSlot;
            case FinalProductSo.ItemType.Boot:
                haveBoot = false;
                return bootSlot;
            default:
                Debug.LogWarning("Unknown product type, cannot place the item.");
                return null;
        }
    }

    public bool HasAvailableSlot<T>() where T : Item
    {
        switch (TypeFinalProductSo)
        {
            case FinalProductSo.ItemType.Helmet:
                return !haveHelmet;
            case FinalProductSo.ItemType.Chest:
                return !haveChest;
            case FinalProductSo.ItemType.Pants:
                return !havePants;
            case FinalProductSo.ItemType.Boot:
                return !haveBoot;
            default:
                return true;
        }
    }
}
