using UnityEngine;
using System;
using System.Linq;
using Utils;

public class AssemblyStation : MonoBehaviour, IInteractable, IInteractableAlt, IHandleItems
{
    public EventHandler OnPutIn;
    public EventHandler OnTakeOut;
    public EventHandler<State> OnStateChanged;

    public enum State
    {
        Idle,
        Processing
    }

    [SerializeField] private Transform itemSlot;
    [SerializeField] private RecipesDictionarySo recipesDictionarySo;
    private readonly StackList<Product> _items = new();
    private State _state;
    private State CurrentState
    {
        get => _state;
        set
        {
            _state = value;
            OnStateChanged?.Invoke(this, _state);
        }
    }
    private AssemblyRecipeSo _assemblyRecipeSo;
    private int _hitToProcess;

    private void Update()
    {
        switch (CurrentState)
        {
            case State.Idle:
                break;
            case State.Processing:
                if (_hitToProcess == 0)
                {
                    _items.ToList().ForEach(i => i.DestroySelf());
                    Item.SpawnItem<Product>(_assemblyRecipeSo.output.prefab, this);
                    CheckForRecipe();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Interact()
    {
        if (CurrentState is not State.Idle) return;

        if (Player.Instance.HandleSystem.HaveAnyItems())
        {
            if (!Player.Instance.HandleSystem.HaveItems<Product>())
            {
                Debug.LogWarning("Station can only hold products!");
                return;
            }
            Player.Instance.HandleSystem.GetItem().SetParent<Product>(this);
            OnPutIn?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            if (_items.Count > 0) 
            {
                Item item = _items.Pop();
                item.SetParent<Item>(Player.Instance.HandleSystem);
                OnTakeOut?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void InteractAlt()
    {
        if (_items.Count > 0)
        {
            if (CurrentState == State.Idle)
            {
                CheckForRecipe();
                if (_hitToProcess != 0)
                {
                    _hitToProcess--;
                }
            }
            else if (CurrentState == State.Processing && _hitToProcess != 0)
            {
                _hitToProcess--;
            }
            else
            {
                CurrentState = State.Idle;
            }
        }
    
    }

    private void CheckForRecipe()
    {
        string[] itemsSo = GetItems<Product>()
            .Cast<Product>()
            .Select((i) => i.ProductSo.itemName)
            .OrderBy(n => n)
            .ToArray();
        AssemblyRecipeSo recipe = recipesDictionarySo.assemblyRecipeSo.FirstOrDefault(r =>
        {
            string[] recipeItemsSo = r.inputs
                .Select(i => i.itemName)
                .OrderBy(n => n)
                .ToArray();
            return itemsSo.SequenceEqual(recipeItemsSo);
        });
        if (recipe is not null)
        {
            CurrentState = State.Processing;
            _hitToProcess = recipe.hitToProcess;
            _assemblyRecipeSo = recipe;
        }
        else
        {
            CurrentState = State.Idle;
        }
    }

    public void AddItem<T>(Item item) where T : Item
    {
        if (typeof(T) != typeof(Product))
        {
            Debug.LogWarning("This station can only hold products!");
            return;
        }
    
        _items.Push(item as Product);
    }

    public Item[] GetItems<T>() where T : Item
    {
        if (typeof(T) != typeof(Product))
        {
            Debug.LogWarning("This station can only hold products!");
            return null;
        }
    
        return _items.Cast<Item>().ToArray();
    }

    public void ClearItem(Item item)
    {
        _items.Remove(item as Product);
    }

    public bool HaveItems<T>() where T : Item
    {
        if (typeof(T) != typeof(Product))
        {
            Debug.LogWarning("This station can only hold products!");
            return false;
        }
    
        return _items.Count > 0;
    }

    public bool HaveAnyItems()
    {
        return _items.Count > 0;
    }

    public Transform GetAvailableItemSlot<T>() where T : Item
    {
        if (typeof(T) != typeof(Product))
        {
            Debug.LogWarning("This station can only hold products!");
            return null;
        }
    
        return itemSlot;
    }

    public bool HasAvailableSlot<T>() where T : Item
    {
        if (typeof(T) != typeof(Product))
        {
            Debug.LogWarning("This station can only hold products!");
            return false;
        }
    
        return true;
    }
}
