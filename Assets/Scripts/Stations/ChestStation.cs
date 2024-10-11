using System;
using UnityEngine;

public class ChestStation : MonoBehaviour, IInteractable, IFocusable
{
    
    public event EventHandler OnFocus;
    public event EventHandler OnStopFocus;
    public event EventHandler OnProductAmountChanged;
    
    [SerializeField] private ProductSo productSo;
    [SerializeField] private int productAmount = 1;
    public void Interact()
    {
        if (!Player.Instance.HandleSystem.HaveAnyItems())
        {
            // If player doesn't have any items, try to take from the chest
            if (productAmount <= 0) return;
            
            Item.SpawnItem<Product>(productSo.prefab, Player.Instance.HandleSystem);
            productAmount--;
            OnProductAmountChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // If player has items, try to put in the chest
            if (!Player.Instance.HandleSystem.HaveItems<Product>()) return;
            
            Product product = Player.Instance.HandleSystem.GetItem<Product>();
            if (product.ProductSo != productSo) return;
            
            productAmount++;
            OnProductAmountChanged?.Invoke(this, EventArgs.Empty);
            product.DestroySelf();
        }
    }
    
    public void Focus()
    {
        OnFocus?.Invoke(this, EventArgs.Empty);
        if (Player.Instance.HandleSystem.HaveBackpackItems())
        {
            // Try to fit backpack items in the chest
            foreach (Item backpackItem in Player.Instance.HandleSystem.GetBackpackItems())
            {
                if (backpackItem is not Product product) continue;
                if (product.ProductSo != productSo) continue;
                
                productAmount++;
                OnProductAmountChanged?.Invoke(this, EventArgs.Empty);
                Player.Instance.HandleSystem.ClearItemFromBackpack(backpackItem);
            }
        }
    }
    
    public void StopFocus()
    {
        OnStopFocus?.Invoke(this, EventArgs.Empty);
    }

    public int GetProductAmount()
    {
        return productAmount;
    }
    
    public ProductSo GetProductSo()
    {
        return productSo;
    }
    public void AddProduct()
    {
        productAmount++;
        OnProductAmountChanged?.Invoke(this, EventArgs.Empty);
    }
}