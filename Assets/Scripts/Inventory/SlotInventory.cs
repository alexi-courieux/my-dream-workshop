﻿using System;
using System.Collections.Generic;
using System.Linq;

public abstract class SlotInventory<T>: IInventory<T>
{
    
    public event EventHandler OnItemAdded;
    public event EventHandler OnItemRemoved;
    public event EventHandler OnSlotAmountChanged;
    
    protected SlotInventoryItem<T>[] ItemSlots;
    private int _maxAmountPerSlot;

    protected SlotInventory(int slotAmount, int maxAmountPerSlot = -1)
    {
        _maxAmountPerSlot = maxAmountPerSlot > 0 ? maxAmountPerSlot : int.MaxValue;
        ItemSlots = new SlotInventoryItem<T>[slotAmount];
    }

    public Dictionary<T, int> GetItems()
    {
        return ItemSlots
            .Where(slot => slot != null)
            .ToDictionary(slot => slot.Item, slot => slot.Amount);
    }

    public SlotInventoryItem<T>[] GetItemSlots()
    {
        return ItemSlots;
    }
    
    public int TryAddItem(T item, int amount)
    {
        if (item is null || amount <= 0) return -1;
        
        int index = Array.FindIndex(ItemSlots, slot => slot is not null && slot.Item.Equals(item));
        if (index >= 0 && ItemSlots[index].Amount < _maxAmountPerSlot) // Item already in inventory & slot not full
        {
            ItemSlots[index].Amount += amount;
            OnItemAdded?.Invoke(this, EventArgs.Empty);
            return index;
        }
        
        index = Array.IndexOf(ItemSlots, default);
        if (index < 0) return -1; // No empty slots
        
        ItemSlots[index] = new SlotInventoryItem<T> { Item = item, Amount = amount };
        OnItemAdded?.Invoke(this, EventArgs.Empty);
        return index;

    }
    
    public void AddItem(int index, T itemToAdd, int amountToAdd)
    {
        if (index < 0 || index >= ItemSlots.Length) throw new Exception("Index out of range");
        if (itemToAdd is null || amountToAdd <= 0) throw new Exception("Item or amount invalid");
        if (ItemSlots[index] != null)
        {
            // Item already in inventory, add amount
            if (ItemSlots[index].Amount + amountToAdd > _maxAmountPerSlot) throw new Exception("Amount exceeds max amount per slot");
            if (!itemToAdd.Equals(ItemSlots[index].Item)) throw new Exception("Item not in specified index");
            ItemSlots[index].Amount += amountToAdd;
        }
        else
        {
            // Add new item
            if (amountToAdd > _maxAmountPerSlot) throw new Exception("Amount exceeds max amount per slot");
            ItemSlots[index] = new SlotInventoryItem<T> { Item = itemToAdd, Amount = amountToAdd };
        }
        OnItemAdded?.Invoke(this, EventArgs.Empty);
    }
    
    public bool CanAddItem(T item)
    {
        if (item is null) return false;
        if (Array.IndexOf(ItemSlots, default) >= 0) return true;
        return ItemSlots.Any(slot => slot.Item.Equals(item) && slot.Amount < _maxAmountPerSlot);
    }
    
    public void RemoveItem(T item, int amount, int index)
    {
        if (item is null || amount <= 0) throw new Exception("Item or amount invalid");
        if (index > ItemSlots.Length) throw new Exception("Index out of range");
        if (index is not -1 && !item.Equals(ItemSlots[index].Item)) throw new Exception("Item not in specified index");

        if (index is -1)
        {
            index = Array.FindIndex(ItemSlots, slot => slot is not null && slot.Item.Equals(item));
            if (index < 0) return; // Item not in inventory
        }

        ItemSlots[index].Amount -= amount;
        if (ItemSlots[index].Amount > 0) {
            // Item still in inventory
            OnItemRemoved?.Invoke(this, EventArgs.Empty);
            return;
        }
        
        ItemSlots[index] = default; // Remove item from inventory
        OnItemRemoved?.Invoke(this, EventArgs.Empty);
    }
    
    public void RemoveItem(T item, int amount)
    {
        RemoveItem(item, amount, -1);
    }
    
    public bool ContainsItem(T item, int amount = 1)
    {
        if (item is null || amount <= 0) return false;
        
        int index = Array.IndexOf(ItemSlots, item);
        return index >= 0 && ItemSlots[index].Amount >= amount;
    }
    
    public void AddSlots(int amount)
    {
        if (amount <= 0) return;
        Array.Resize(ref ItemSlots, ItemSlots.Length + amount);
        OnSlotAmountChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public bool TryRemoveSlots(int amount)
    {
        if (amount <= 0 || ItemSlots.Length - amount < 0) return false; // Cannot remove more slots than available

        var items = ItemSlots.Where(slot => slot != null).ToArray();
        if (items.Length > ItemSlots.Length - amount) return false; // Cannot remove slots with items in them

        var newSlots = new SlotInventoryItem<T>[ItemSlots.Length - amount];
        Array.Copy(items, newSlots, items.Length);
        ItemSlots = newSlots;

        OnSlotAmountChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }
    
    public SlotInventoryItem<T> GetSlot(int index)
    {
        return index >= 0 && index < ItemSlots.Length ? ItemSlots[index] : default;
    }
    
    public int GetSlotAmount()
    {
        return ItemSlots.Length;
    }
    
    public int[] GetItemSlots(T item)
    {
        return ItemSlots
            .Select((slot, index) => new { slot, index })
            .Where(x => x.slot is not null && x.slot.Item.Equals(item))
            .Select(x => x.index)
            .OrderBy(x => x)
            .ToArray();
    }
    
    protected void SortItems(Func<KeyValuePair<T, int>, object> keySelector, bool ascending = true)
    {
        var items = GetItems().ToList();
        items.Sort((x, y) => ascending
            ? Comparer<object>.Default.Compare(keySelector(x), keySelector(y))
            : Comparer<object>.Default.Compare(keySelector(y), keySelector(x)));
        
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            ItemSlots[i] = i < items.Count ? new SlotInventoryItem<T> { Item = items[i].Key, Amount = items[i].Value } : default;
        }
    }
}