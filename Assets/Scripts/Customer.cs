using System;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{ 
    public event EventHandler OnOrder;
    public event EventHandler OnLeave;
    
    [SerializeField] protected NavMeshAgent agent;
    
    private Action _onDestinationReached;
    private Transform _destination;
    protected CheckoutStation CheckoutStation;
    protected ProductSo Order;

    public void Initialize(CheckoutStation checkoutStation)
    {
        agent.Warp(CustomerManager.Instance.GetSpawnPoint().position);
        
        CheckoutStation = checkoutStation;
        checkoutStation.Add(this);
        MoveToThen(CheckoutStation.GetPosition(this), TryToOrder);
        RegisterToCheckoutStation();
    }
    
    private void Update()
    {
        if(_destination is null) return;
        if (agent.remainingDistance > 1f) return;
        
        DestinationReached();
    }
    
    private void RegisterToCheckoutStation()
    {
        CheckoutStation.OnCustomerCheckout += CheckoutStation_OnCustomerCheckout;
    }
    
    private void UnregisterFromCheckoutStation()
    {
        CheckoutStation.OnCustomerCheckout -= CheckoutStation_OnCustomerCheckout;
    }
    
    private void CheckoutStation_OnCustomerCheckout(object sender, Customer customer)
    {
        if (customer == this)
        {
            UnregisterFromCheckoutStation();
            OnLeave?.Invoke(this, EventArgs.Empty);
            MoveToThen(CustomerManager.Instance.GetDispawnPoint(), () =>
            {
                CustomerManager.Dispawn(this);
            });
            return;
        }
        MoveToThen(CheckoutStation.GetPosition(this), TryToOrder);
    }
    
    private void DestinationReached()
    {
        _onDestinationReached?.Invoke();
        _onDestinationReached = null;
        agent.updateRotation = false;
        transform.rotation = _destination.rotation;
        _destination = null;
    }

    private void MoveTo(Transform newDestination)
    {
        _destination = newDestination;
        agent.SetDestination(newDestination.position);
        agent.updateRotation = true;
    }
    
    private void MoveToThen(Transform newDestination, Action onDestinationReached)
    {
        MoveTo(newDestination);
        _onDestinationReached = onDestinationReached;
    }

    protected void TryToOrder()
    {
        if (!CheckoutStation.IsFirst(this)) return;
        
        ProductSo[] sellableProducts = OrderManager.Instance.GetSellableProducts();
        Order = sellableProducts[UnityEngine.Random.Range(0, sellableProducts.Length)];
        
        OnOrder?.Invoke(this, EventArgs.Empty);
    }
    
    public ProductSo GetOrder()
    {
        return Order;
    }
}