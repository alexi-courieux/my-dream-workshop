using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SingleItemChestStation : MonoBehaviour, IInteractable, IUseable, IFocusable
{

    public event EventHandler OnFocus;
    public event EventHandler OnStopFocus;
    public event EventHandler OnProductAmountChanged;

    [SerializeField] private ProductSo productSo;
    [SerializeField] private int productAmount = 1;

    public void Use()
    {
        // Try to take from the chest
        if (productAmount <= 0) return;
        if (!Player.Instance.HandleSystem.HaveSpace(productSo)) return;

        Item.SpawnItem(productSo.prefab, Player.Instance.HandleSystem);
        productAmount--;
        OnProductAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Interact()
    {
        // Try to put in the chest
        Item playerItem = Player.Instance.HandleSystem.GetSelectedItem();
        if (playerItem is not Product playerProduct) return;
        if (!playerProduct.ProductSo.Equals(productSo)) return;

        productAmount++;
        OnProductAmountChanged?.Invoke(this, EventArgs.Empty);
        playerItem.DestroySelf();
    }

    public void Focus()
    {
        OnFocus?.Invoke(this, EventArgs.Empty);
    }

    public void StopFocus()
    {
        OnStopFocus?.Invoke(this, EventArgs.Empty);
    }

    public int GetProductAmount()
    {
        return productAmount;
    }

    public ItemSo GetProductSo()
    {
        return productSo;
    }
}