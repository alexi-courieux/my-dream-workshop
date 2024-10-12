using System;
using System.Linq;
using UnityEngine;

public class AnvilStation : MonoBehaviour, IInteractable, IInteractableAlt, IHandleItems, IHasProgress, ISelectableRecipe, IInteractableNext, IInteractablePrevious, IFocusable
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<RecipeSelectedEventArgs> OnRecipeSelected;
    public event EventHandler OnFocus;
    public event EventHandler OnStopFocus;

    private enum State
    {
        Idle,
        Processing
    }

    [SerializeField] private Transform itemSlot;
    [SerializeField] private RecipesDictionarySo recipesDictionarySo;
    private AnvilRecipeSo _anvilRecipeSo;
    private AnvilRecipeSo[] _availableRecipes;
    private Product _product;
    private int _hitToProcess;
    private State _state;

    private void Start()
    {
        _state = State.Idle;
    }

    public void Interact()
    {
        bool isPlayerHoldingProduct = Player.Instance.HandleSystem.HaveItems<Product>();
        
        if (HaveItems<Product>())
        {
            if (isPlayerHoldingProduct) return;
            _product.SetParent(Player.Instance.HandleSystem);
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
            ClearRecipe();
        }
        else
        {
            if (!isPlayerHoldingProduct) return;
            
            _state = State.Idle;
            Item product = Player.Instance.HandleSystem.GetItem();
            product.SetParent(this);
            CheckForRecipes();
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
            
        }
    }

    public void InteractAlt()
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
        Item.SpawnItem<Product>(_anvilRecipeSo.output.prefab, this);
        CheckForRecipes();
        _state = State.Idle;
    }
    
    private void CheckForRecipes()
    {
        _availableRecipes = recipesDictionarySo.anvilRecipeSo.Where(r => r.input == _product.ProductSo).ToArray();
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
        OnRecipeSelected?.Invoke(this, new RecipeSelectedEventArgs(_anvilRecipeSo.output, _availableRecipes.Length));
    }
    
    private void ClearRecipe()
    {
        _anvilRecipeSo = null;
        OnRecipeSelected?.Invoke(this, new RecipeSelectedEventArgs(null, 0));
    }
}
