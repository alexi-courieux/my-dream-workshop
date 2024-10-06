using UnityEngine;
using System;
using System.Linq;
using Utils;

public class AssemblyStation : MonoBehaviour, IInteractable, IInteractableAlt, IHandleItems, IHasProgress
{
    public EventHandler OnPutIn;
    public EventHandler OnTakeOut;
    public EventHandler<State> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

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
    private int _hitToProcessMax = int.MaxValue;
    private int _hitToProcess;

    private void Update()
    {
        switch (CurrentState)
        {
            case State.Idle:
                break;
            case State.Processing:
                if (_hitToProcess == _hitToProcessMax)
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

            _hitToProcess = 0;

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
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
                if (_hitToProcess == 0 && _assemblyRecipeSo is not null)
                {
                    _hitToProcess++;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)_hitToProcess / _hitToProcessMax
                    });
                }
            }
            else if (CurrentState == State.Processing && _hitToProcess != 0)
            {
                _hitToProcess++;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = (float)_hitToProcess / _hitToProcessMax
                });
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
            _hitToProcessMax = recipe.hitToProcess;
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
