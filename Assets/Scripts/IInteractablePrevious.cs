using System;
/// <summary>
/// Interface for objects that can be interacted with (previous)
/// </summary>
public interface IInteractablePrevious
{
       public void InteractPrevious();
       public event EventHandler OnPrevious;
}