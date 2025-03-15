using System;
using System.Collections.Generic;
using UnityEngine;
public class ChestStation : MonoBehaviour, IInteractable, IUseable, IFocusable, IInteractablePrevious, IInteractableNext, IHasProgress,
    ISelectableProduct
{
    public event EventHandler OnFocus;
    public event EventHandler OnStopFocus;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<SelectedProductEventArgs> OnProductSelected;

    private const int Capacity = 10;

    private List<ProductSo> _products;
    private int _index;


    private void Start()
    {
        _products = new List<ProductSo>();
        _index = 0;
    }

    public void Use()
    {
        // Try to take from the chest
        if (_products.Count <= 0) return;
        if (!Player.Instance.HandleSystem.HaveSpace(_products[_index])) return;
        Item.SpawnItem(_products[_index].prefab, Player.Instance.HandleSystem);
        _products.RemoveAt(_index);
        if (_index >= _products.Count)
        {
            _index = 0;
        }
        SelectProduct();
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = (float) _products.Count / Capacity
        });
    }

    public void Interact()
    {
        // Try to put in the chest
        if (Player.Instance.HandleSystem.HaveAnyItemSelected())
        {
            if (_products.Count >= Capacity) return;

            Item playerItem = Player.Instance.HandleSystem.GetSelectedItem();
            if (playerItem is not Product playerProduct) return;
            _products.Add(playerProduct.ProductSo);
            _index = _products.Count - 1;
            SelectProduct();
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float) _products.Count / Capacity
            });
            playerItem.DestroySelf();
        }
    }
    public void InteractPrevious()
    {
        _index--;
        if (_index < 0)
        {
            _index = _products.Count - 1;
        }
        SelectProduct();
    }
    public void InteractNext()
    {
        _index++;
        if (_index >= _products.Count)
        {
            _index = 0;
        }
        SelectProduct();
    }
    
    private void SelectProduct()
    {
        if (_products.Count <= 0)
        {
            OnProductSelected?.Invoke(this, new SelectedProductEventArgs(null, 0));
            return;
        }
        ProductSo productSo = _products[_index];
        OnProductSelected?.Invoke(this, new SelectedProductEventArgs(productSo, _products.Count));
    }
    public void Focus()
    {
        OnFocus?.Invoke(this, EventArgs.Empty);
    }
    public void StopFocus()
    {
        OnStopFocus?.Invoke(this, EventArgs.Empty);
    }
}