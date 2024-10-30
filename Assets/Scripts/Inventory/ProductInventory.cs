using System;
using System.Linq;
public class ProductInventory : SlotInventory<ProductSo>
{
    public ProductInventory(int slotAmount, int maxAmountPerSlot = -1) : base(slotAmount, maxAmountPerSlot)
    {
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
    
    public int[] Search(string search)
    {
        var items = GetItems().Where(x => 
            x.Key.itemName.Contains(search) 
            || x.Key.types.Any(tag => tag.GetPath().Contains(search))).ToArray();
        return items.Select(x => Array.IndexOf(ItemSlots, x.Key)).ToArray();
    }
}