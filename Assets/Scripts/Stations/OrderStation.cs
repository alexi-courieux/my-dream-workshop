using System;
using UnityEngine;

public class OrderStation : MonoBehaviour, IUseable, IFocusable
{
    public event EventHandler OnUse;
    public event EventHandler OnFocus;
    public event EventHandler OnStopFocus;
    
    public void Use()
    {
        OnUse?.Invoke(this, EventArgs.Empty);
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