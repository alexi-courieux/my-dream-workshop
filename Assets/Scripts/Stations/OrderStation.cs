using System;
using UnityEngine;

public class OrderStation : MonoBehaviour, IUseable
{
    public event EventHandler OnUse;
    
    public void Use()
    {
        OnUse?.Invoke(this, EventArgs.Empty);
    }
}