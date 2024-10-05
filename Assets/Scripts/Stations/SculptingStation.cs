using System;
using System.Linq;
using UnityEngine;

public class SculptingStation : MonoBehaviour, IInteractable, IInteractableAlt, IHandleItems
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
    private SculptingRecipeSo _sculptingRecipeSo;
    private Product _product;
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
                if (_hitToProcess == 0)
                {
                    _product.DestroySelf();
                    Item.SpawnItem<Product>(_sculptingRecipeSo.output.prefab, this);
                    CheckForRecipe();
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
        }
    }

    public void InteractAlt()
    {
        if (_product is not null)
        {
            if (CurrentState == State.Idle)
            {
                CheckForRecipe();
                if (_hitToProcess != 0)
                {
                    _hitToProcess--;
                }
            }
            else 
            {
                _hitToProcess--;
            }
        }
        else 
        {
            CurrentState = State.Idle;
        }
    }
    private void CheckForRecipe()
    {
        SculptingRecipeSo recipe = recipesDictionarySo.sculptingRecipeSo.FirstOrDefault(r => r.input == _product.ProductSo);
        if (recipe is not null)
        {
            CurrentState = State.Processing;
            _hitToProcess = recipe.hitToProcess;
            _sculptingRecipeSo = recipe;
        }
        else
        {
            CurrentState = State.Idle;
        }
    }

    public void AddItem<T>(Item item) where T : Item
    {
        if(typeof(T) != typeof(Product))
        {
            throw new Exception("This station can only hold products!");
        }
    
        _product = item as Product;
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

    public void ClearItem(Item item)
    {
        if (_product == item as Product)
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
