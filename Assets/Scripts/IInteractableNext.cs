using System;
using JetBrains.Annotations;
/// <summary>
/// Interface for objects that can be interacted with (next)
/// </summary>
public interface IInteractableNext
{
       public void InteractNext();
       public event EventHandler OnNext;
}