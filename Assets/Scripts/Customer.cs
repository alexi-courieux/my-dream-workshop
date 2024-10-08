using System;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{ 
    public event EventHandler OnOrder;
    public event EventHandler OnLeave;
    
    [SerializeField] private NavMeshAgent agent;
    
    private Action _onDestinationReached;
    private Transform _destination;
    private CheckoutStation _checkoutStation;
    private ProductSo _order;

    public void Initialize(CheckoutStation checkoutStation)
    {
        agent.Warp(CustomerManager.Instance.GetSpawnPoint().position);
        
        _checkoutStation = checkoutStation;
        checkoutStation.Add(this);
        MoveToThen(_checkoutStation.GetPosition(this), TryToOrder);
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
        _checkoutStation.OnCustomerCheckout += CheckoutStation_OnCustomerCheckout;
    }
    
    private void UnregisterFromCheckoutStation()
    {
        _checkoutStation.OnCustomerCheckout -= CheckoutStation_OnCustomerCheckout;
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
        MoveToThen(_checkoutStation.GetPosition(this), TryToOrder);
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

    private void TryToOrder()
    {
        if (!_checkoutStation.IsFirst(this)) return;
        
        ProductSo[] sellableProducts = OrderManager.Instance.GetSellableProducts().products;
        _order = sellableProducts[UnityEngine.Random.Range(0, sellableProducts.Length)];
        OnOrder?.Invoke(this, EventArgs.Empty);
    }
    
    public ProductSo GetOrder()
    {
        return _order;
    }
}