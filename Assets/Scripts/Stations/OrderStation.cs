using System;
using UnityEngine;

public class OrderStation : MonoBehaviour, IInteractable
{
    public event EventHandler OnInteract;
    
    [SerializeField] private OrderUI orderUi;
    
    public void Interact()
    {
        orderUi.Show();
        OnInteract?.Invoke(this, EventArgs.Empty);
    }
}