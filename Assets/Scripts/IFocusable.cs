using System;
/// <summary>
/// Interface for objects that can be interacted with
/// </summary>
public interface IFocusable
{
       public void Focus();
       public void StopFocus();
       public event EventHandler OnFocus;
       public event EventHandler OnStopFocus;
}