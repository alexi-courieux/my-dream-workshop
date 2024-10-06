using System;
using System.Linq;
using UnityEngine;

public class AnvilStation : MonoBehaviour, IInteractable, IInteractableAlt, IHandleItems, IHasProgress
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
    private AnvilRecipeSo _anvilRecipeSo;
    private Product _product;
    private int _hitToProcessMax = int.MaxValue;
    private int _hitToProcess;

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

    private void Update()
    {
        switch (CurrentState)
        {
            case State.Idle:
                break;
            case State.Processing:
                if (_hitToProcess == _hitToProcessMax)
                {
                    _product.DestroySelf();
                    Item.SpawnItem<Product>(_anvilRecipeSo.output.prefab, this);
                    _state = State.Idle;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    public void Interact()
    {
        bool isPlayerHoldingProduct = Player.Instance.HandleSystem.HaveItems<Product>();
        if (HaveItems<Product>())
        {
            if (isPlayerHoldingProduct) return;
            _product.SetParent<Item>(Player.Instance.HandleSystem);
            OnTakeOut?.Invoke(this, EventArgs.Empty);
            _state = State.Idle;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
        }
        else
        {
            if (!isPlayerHoldingProduct) return;
            Item item = Player.Instance.HandleSystem.GetItem();
            if (item is not Product product)
            {
                Debug.LogWarning("Station can only hold products!");
                return;
            }
            product.SetParent<Product>(this);
            OnPutIn?.Invoke(this, EventArgs.Empty);
            _hitToProcess = 0;

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
        }
    }

    public void InteractAlt()
    {
        if (_product is not null)
        {
            if (CurrentState == State.Idle)
            {
                CheckForRecipe();
                if (_hitToProcess == 0)
                {
                    _hitToProcess++;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)_hitToProcess / _hitToProcessMax
                    });
                }
            }
            else 
            {
                _hitToProcess++;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = (float)_hitToProcess / _hitToProcessMax
                });
            }
        }
        else 
        {
            CurrentState = State.Idle;
        }
    }
    private void CheckForRecipe()
    {
        AnvilRecipeSo recipe = recipesDictionarySo.anvilRecipeSo.FirstOrDefault(r => r.input == _product.ProductSo);
        if (recipe is not null)
        {
            CurrentState = State.Processing;
            _hitToProcessMax = recipe.hitToProcess;
            _anvilRecipeSo = recipe;
        }
        else
        {
            CurrentState = State.Idle;
        }
    }

    public void AddItem<T>(Item newItem) where T : Item
    {
        if(typeof(T) != typeof(Product))
        {
            throw new Exception("This station can only hold products!");
        }
    
        _product = newItem as Product;
    }

    public Item[] GetItems<T>() where T : Item
    {
        if (typeof(T) == typeof(Product))
        {
            return new Item[]{_product};
        }
        Debug.LogWarning($"This station doesn't have items of the specified type : {typeof(T)}");
        return new Item[]{};
    }

    public void ClearItem(Item itemToClear)
    {
        if (_product == itemToClear as Product)
        {
            _product = null;
        }
    }

    public bool HaveItems<T>() where T : Item
    {
        if (typeof(T) == typeof(Product))
        {
            return _product is not null;
        }
        Debug.LogWarning($"This station doesn't have items of the specified type : {typeof(T)}");
        return false;
    }

    public bool HaveAnyItems()
    {
        return _product is not null;
    }

    public Transform GetAvailableItemSlot<T>() where T : Item
    {
        if (typeof(T) == typeof(Product))
        {
            return itemSlot;
        }
        Debug.LogWarning($"This station doesn't have items of the specified type : {typeof(T)}");
        return null;
    }

    public bool HasAvailableSlot<T>() where T : Item
    {
        if(typeof(T) == typeof(Product))
        {
            return _product is null;
        }
        Debug.LogWarning($"This station doesn't have items of the specified type : {typeof(T)}");
        return false;
    }
}
