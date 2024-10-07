using System;
using System.Linq;
using UnityEngine;

public class WoodcuttingStation : MonoBehaviour, IInteractable, IInteractableAlt, IHandleItems, IHasProgress
{
    public event EventHandler OnPutIn;
    public event EventHandler OnTakeOut;
    public event EventHandler OnProcessing;
    public event EventHandler OnStopProcessing;
    public event EventHandler<State> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public enum State
    {
        Idle,
        Processing
    }

    [SerializeField] private Transform itemSlot;
    [SerializeField] private RecipesDictionarySo recipesDictionarySo;
    private WoodcuttingRecipeSo _woodcuttingRecipeSo;
    private Product _product;
    private float _timeToProcessMax = float.MaxValue;
    private float _timeToProcess;

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
                _timeToProcess += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = _timeToProcess / _timeToProcessMax
                });
                if (_timeToProcess >= _timeToProcessMax)
                {
                    _product.DestroySelf();
                    Item.SpawnItem<Product>(_woodcuttingRecipeSo.output.prefab, this);
                    _state = State.Idle;
                    OnStopProcessing?.Invoke(this, EventArgs.Empty);
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = 0f
                    });
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
            OnStopProcessing?.Invoke(this, EventArgs.Empty);
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
            _timeToProcess = 0f;

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
        }
    }

    public void InteractAlt()
    {
        if (_product is not null)
        {
            if (CurrentState is not State.Idle) return;
            
            CheckForRecipe();
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = _timeToProcess / _timeToProcessMax
            });
            OnProcessing?.Invoke(this, EventArgs.Empty);
        }
        else 
        {
            CurrentState = State.Idle;
            OnStopProcessing?.Invoke(this, EventArgs.Empty);
        }
    }
    private void CheckForRecipe()
    {
        WoodcuttingRecipeSo recipe = recipesDictionarySo.woodcuttingRecipes.FirstOrDefault(r => r.input == _product.ProductSo);
        if (recipe is not null)
        {
            CurrentState = State.Processing;
            _timeToProcessMax = recipe.timeToProcess;
            _woodcuttingRecipeSo = recipe;
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
