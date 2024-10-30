using System;
using AshLight.BakerySim;
using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable, IInteractableAlt, IFocusable
{
    public event EventHandler OnFocus;
    public event EventHandler OnStopFocus;
    
    private IHandleItems _parent;

    public static Item SpawnItem(Transform itemPrefab, IHandleItems parent)
    {
        Transform itemTransform = Instantiate(itemPrefab);
        Item item = itemTransform.GetComponent<Item>();
        item.SetParent(parent);
        return item;
    }

    /// <summary>
    /// Try to change the parent of this item to another one, it must have an available slot or be null
    /// </summary>
    /// <param name="targetParent">new parent</param>
    /// <returns>true if the parent have changed</returns>
    /// <exception cref="ArgumentException">The parent does not have available slot to take the item</exception>
    public void SetParent(IHandleItems targetParent)
    {
        if (targetParent is null)
        {
            throw new Exception("SetParent<T> must have a non-null parent, use Drop or DestroySelf instead");
        }

        if (!targetParent.HasAvailableSlot(this))
        {
            throw new ArgumentException("The parent must have an available slot or be null");
        }

        _parent?.ClearItem(this);

        _parent = targetParent;

        if (_parent is not null)
        {
            transform.parent = _parent.GetAvailableItemSlot(this);
            _parent.AddItem(this);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    public void DestroySelf()
    {
        _parent.ClearItem(this);
        Destroy(gameObject);
    }

    public void Interact()
    {
        if (_parent is IInteractable interactableParent)
        {
            interactableParent.Interact();
        }
        else
        {
            SetParent(Player.Instance.HandleSystem);
        }
    }

    public void InteractAlt()
    {
        if (_parent is IInteractableAlt interactableParent)
        {
            interactableParent.InteractAlt();
        }
    }

    public void Focus()
    {
        OnFocus?.Invoke(this, EventArgs.Empty);
        if (_parent is IFocusable focusableParent)
        {
            focusableParent.Focus();
        }
    }

    public void StopFocus()
    {
        OnStopFocus?.Invoke(this, EventArgs.Empty);
        if (_parent is IFocusable focusableParent)
        {
            focusableParent.StopFocus();
        }
    }
}