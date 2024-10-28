using System;
using System.Collections.Generic;
using System.Linq;
public class Inventory
{
    private Dictionary<ProductSo, int> slots;
    private int slotAmount;
    
    public Inventory(int slotAmount)
    {
        this.slotAmount = slotAmount;
        slots = new Dictionary<ProductSo, int>();
    }
    
    public Dictionary<ProductSo, int> GetProducts()
    {
        return slots;
    }
    
    public bool TryAddProduct(ProductSo product, int amount)
    {
        if (slots.ContainsKey(product))
        {
            slots[product] += amount;
            return true;
        }
        
        if (slots.Count >= slotAmount) return false;
        
        slots.Add(product, amount);
        return true;
    }
    
    public void RemoveProduct(ProductSo product, int amount)
    {
        if (!slots.ContainsKey(product)) return;
        
        slots[product] -= amount;
        if (slots[product] <= 0)
        {
            slots.Remove(product);
        }
    }
    
    public bool ContainsProduct(ProductSo product, int amount)
    {
        if (slots.TryGetValue(product, out int invAmount))
        {
            return invAmount >= amount;
        }
        return false;
    }
    
    private void SortItems(Func<KeyValuePair<ProductSo, int>, object> keySelector, bool ascending = true)
    {
        slots = ascending
            ? slots.OrderBy(keySelector).ToDictionary(x => x.Key, x => x.Value)
            : slots.OrderByDescending(keySelector).ToDictionary(x => x.Key, x => x.Value);
    }
    
    public void SortItemsByName(bool ascending = true)
    {
        SortItems(x => x.Key.itemName, ascending);
    }
    
    public void SortItemsByTags(bool ascending = true)
    {
        SortItems(x => x.Key.types[0].GetPath(), ascending);
    }
    
    public void SortItemsByAmount(bool ascending = true)
    {
        SortItems(x => x.Value, ascending);
    }
}