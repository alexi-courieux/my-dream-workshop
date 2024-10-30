using UnityEngine;
public class SlotInventoryItem<T>
{
    public T Item { get; set; }
    
    private int amount;
    
    public int Amount
    {
        get => amount;
        set => amount = value;
    }
}