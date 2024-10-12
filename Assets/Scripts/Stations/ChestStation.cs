using System;
using System.Collections.Generic;
using UnityEngine;
public class ChestStation : MonoBehaviour, IInteractable, IFocusable, IInteractablePrevious, IInteractableNext, IHasProgress,
    ISelectablProduct
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

    public void Interact()
    {
        if (Player.Instance.HandleSystem.HaveAnyItems())
        {
            if (_products.Count >= Capacity) return;

            Item playerItem = Player.Instance.HandleSystem.GetItem();
            if (playerItem is not Product
                && playerItem is not FinalProduct) return;
            switch (playerItem)
            {
                case Product product:
                    _products.Add(product.ProductSo);
                    break;
                case FinalProduct finalProduct:
                    _products.Add(finalProduct.FinalProductSo);
                    break;
            }
            _index = _products.Count - 1;
            SelectProduct();
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float) _products.Count / Capacity
            });
            playerItem.DestroySelf();
        }
        else
        {
            if (_products.Count <= 0) return;

            ProductSo productSo = _products[_index];
            Item.SpawnItem(productSo.prefab, Player.Instance.HandleSystem);
            _products.RemoveAt(_index);
            if (_index >= _products.Count)
            {
                _index = _products.Count - 1;
            }
            SelectProduct();
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float) _products.Count / Capacity
            });
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