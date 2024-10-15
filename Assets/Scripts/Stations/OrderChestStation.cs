using System;
using System.Collections.Generic;
using UnityEngine;
public class OrderChestStation : MonoBehaviour, IInteractable, IFocusable, IInteractablePrevious, IInteractableNext,
    ISelectablProduct
{
    public event EventHandler OnFocus;
    public event EventHandler OnStopFocus;
    public event EventHandler<SelectedProductEventArgs> OnProductSelected;
    public event EventHandler<ProductSo> OnProductTaken; 

    private List<ProductSo> _products;
    private int _index;


    private void Start()
    {
        _products = new List<ProductSo>();
        _index = 0;
    }

    public void Interact()
    {
        if (Player.Instance.HandleSystem.HaveAnyItems()) return;
        if (_products.Count <= 0) return;

        ProductSo productSo = _products[_index];
        Item.SpawnItem(productSo.prefab, Player.Instance.HandleSystem);
        _products.RemoveAt(_index);
        OnProductTaken?.Invoke(this, productSo);
        if (_index >= _products.Count)
        {
            _index = 0;
        }
        SelectProduct();
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
    
    public void AddProduct(ProductSo productSo)
    {
        _products.Add(productSo);
        OnProductSelected?.Invoke(this, new SelectedProductEventArgs(productSo, _products.Count));
    }
}