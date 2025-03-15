using System.Collections.Generic;
using UnityEngine;
public interface IInventory<T>
{
    public int TryAddItem(T item, int amount);
    public void RemoveItem(T item, int amount);
    public bool ContainsItem(T product, int amount);
    public Dictionary<T, int> GetItems();
}