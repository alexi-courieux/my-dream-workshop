using System;
using System.Linq;
using UnityEngine;

public class AnvilStation : MonoBehaviour, IInteractable, IUseable, IHandleItems, IHasProgress, ISelectableProduct, IInteractableNext, IInteractablePrevious, IFocusable
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<SelectedProductEventArgs> OnProductSelected;
    public event EventHandler OnFocus;
    public event EventHandler OnStopFocus;

    private enum State
    {
        Idle,
        Processing
    }

    [SerializeField] private Transform itemSlot;
    private AnvilRecipeSo _anvilRecipeSo;
    private AnvilRecipeSo[] _availableRecipes;
    private Product _product;
    private int _hitToProcess;
    private State _state;

    private void Start()
    {
        _state = State.Idle;
        OrderManager.Instance.OnRecipeBuy += OrderManager_OnRecipeBuy;
    }

    private void OrderManager_OnRecipeBuy(object sender, EventArgs e)
    {
        CheckForRecipes();
    }

    public void Interact()
    {
        bool isPlayerHoldingSomething = Player.Instance.HandleSystem.HaveAnyItemSelected();
        
        if (HaveItems<Product>())
        {
            if (isPlayerHoldingSomething) return;
            _product.SetParent(Player.Instance.HandleSystem);
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
            ClearRecipe();
        }
        else
        {
            if (!isPlayerHoldingSomething) return;
            if (!Player.Instance.HandleSystem.HaveItemSelected<Product>()) return;
            
            _state = State.Idle;
            Item product = Player.Instance.HandleSystem.GetSelectedItem();
            product.SetParent(this);
            CheckForRecipes();
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
            
        }
    }

    public void Use()
    {
        if (_anvilRecipeSo is null) return;
        
        if(_state == State.Idle) _state = State.Processing;
        
        _hitToProcess--;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = 1 - (float)_hitToProcess / _anvilRecipeSo.hitToProcess
        });

        if (_hitToProcess <= 0)
        {
            Transform();
        }
        
    }

    private void Transform()
    {
        _product.DestroySelf();
        Item.SpawnItem(_anvilRecipeSo.output.prefab, this);
        CheckForRecipes();
        _state = State.Idle;
    }
    
    private void CheckForRecipes()
    {
        if(_product is null) return;
        
        _availableRecipes = RecipeManager.Instance.GetRecipes<AnvilRecipeSo>();
        _availableRecipes = _availableRecipes.Where(r => r.input == _product.ProductSo).ToArray();
        if (_availableRecipes.Length > 0)
        {
            SelectRecipe(_availableRecipes[0]);
        } 
        else
        {
            ClearRecipe();
        }
    }

    public void AddItem(Item newItem)
    {
        if(newItem is not Product product)
        {
            throw new Exception("This station can only hold products!");
        }
    
        _product = product;
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
        return false;
    }

    public bool HaveAnyItems()
    {
        return _product is not null;
    }

    public Transform GetAvailableItemSlot(Item newItem)
    {
        if (newItem is Product)
        {
            return itemSlot;
        }
        return null;
    }

    public bool HasAvailableSlot(Item item)
    {
        if(item is Product)
        {
            return _product is null;
        }
        return false;
    }
    public void InteractNext()
    {
        if (_state == State.Processing) return;
        if (_anvilRecipeSo is null) return;
        
        int index = Array.IndexOf(_availableRecipes, _anvilRecipeSo);
        index++;
        if (index >= _availableRecipes.Length)
        {
            index = 0;
        }
        SelectRecipe(_availableRecipes[index]);
    }
    public void InteractPrevious()
    {
        if (_state == State.Processing) return;
        if (_anvilRecipeSo is null) return;
        
        int index = Array.IndexOf(_availableRecipes, _anvilRecipeSo);
        index--;
        if (index < 0)
        {
            index = _availableRecipes.Length - 1;
        }
        SelectRecipe(_availableRecipes[index]);
    }
    public void Focus()
    {
        OnFocus?.Invoke(this, EventArgs.Empty);
    }
    public void StopFocus()
    {
        OnStopFocus?.Invoke(this, EventArgs.Empty);
    }

    private void SelectRecipe(AnvilRecipeSo recipe)
    {
        _anvilRecipeSo = recipe;
        _hitToProcess = recipe.hitToProcess;
        OnProductSelected?.Invoke(this, new SelectedProductEventArgs(_anvilRecipeSo.output, _availableRecipes.Length));
    }
    
    private void ClearRecipe()
    {
        _anvilRecipeSo = null;
        OnProductSelected?.Invoke(this, new SelectedProductEventArgs(null, 0));
    }
}
