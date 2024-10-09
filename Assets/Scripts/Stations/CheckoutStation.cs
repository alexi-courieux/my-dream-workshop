using System;
using UnityEngine;

public class CheckoutStation : MonoBehaviour, IInteractable
{
    
    public event EventHandler<Customer> OnCustomerCheckout; 
    
    [SerializeField] private Transform[] queuePositions;
    
    private WaitingQueue<Customer> _customerQueue;

    private void Start()
    {
        _customerQueue = new WaitingQueue<Customer>(queuePositions);
    }
    
    public void Add(Customer customer)
    {
        _customerQueue.Add(customer);
    }
    
    public Transform GetPosition(Customer customer)
    {
        return _customerQueue.GetPosition(customer);
    }
    
    public void Interact()
    {
        if (_customerQueue.Count <= 0) return;
        TryCompleteOrder();
    }
    
    public bool IsFirst(Customer customer)
    {
        return _customerQueue.PeekFirst() == customer;
    }
    
    private void TryCompleteOrder()
    {
        if (!Player.Instance.HandleSystem.HaveItems<FinalProduct>()) return;
        
        Customer customer = _customerQueue.PeekFirst();
        if (_customerQueue.PeekFirst().GetOrder() is null) return;

        FinalProduct playerProduct = Player.Instance.HandleSystem.GetItem<FinalProduct>();
        ProductSo order = customer.GetOrder();
        if (order != playerProduct.FinalProductSo) return;
        
        OrderManager.Instance.Sell(order);
        playerProduct.DestroySelf();
        _customerQueue.Shift();
        OnCustomerCheckout?.Invoke(this, customer);
    }
}