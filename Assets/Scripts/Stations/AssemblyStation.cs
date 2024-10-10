using UnityEngine;
using System;
using System.Linq;
using Utils;

public class AssemblyStation : MonoBehaviour, IInteractable, IInteractableAlt, IHandleItems, IHasProgress, ISelectableRecipe, IInteractablePrevious, IInteractableNext, IFocusable
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

    [SerializeField] private Transform[] itemSlots;
    [SerializeField] private Transform finalProductSlot;
    [SerializeField] private RecipesDictionarySo recipesDictionarySo;
    
    private readonly StackList<Product> _items = new();
    private FinalProduct _finalProduct;
    private State _state;
    private AssemblyRecipeSo[] _availableRecipes;
    private AssemblyRecipeSo _selectedRecipe;
    private int _hitToProcess;
    private int _capacity;

    private void Awake() {
        _capacity = itemSlots.Length;
    }

    private void Start()
    {
        _state = State.Idle;
    }

    public void Interact()
    {
        if (Player.Instance.HandleSystem.HaveAnyItems())
        {
            if (_finalProduct is not null) return;
            if (!Player.Instance.HandleSystem.HaveItems<Product>()) return;
            if (!HasAvailableSlot<Product>()) return;
            
            Player.Instance.HandleSystem.GetItem().SetParent<Product>(this);

            CheckRecipes();
            _state = State.Idle;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
        }
        else
        {
            if (_finalProduct is not null)
            {
                _finalProduct.SetParent<Item>(Player.Instance.HandleSystem);
                return;
            }

            if (_items.Count <= 0) return;
            
            Item item = _items.Pop();
            item.SetParent<Item>(Player.Instance.HandleSystem);
            _state = State.Idle;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
            CheckRecipes();
        }
    }

    private void CheckRecipes()
    {
        ProductSo[] productsSo = GetItems<Product>()
            .Cast<Product>()
            .Select(i => i.ProductSo)
            .OrderBy(i => i.itemName)
            .ToArray();
        _availableRecipes = recipesDictionarySo.assemblyRecipeSo
            .Where(r =>
            {
                ProductSo[] recipeInputs = r.inputs
                    .OrderBy(i => i.itemName)
                    .ToArray();
                return productsSo.SequenceEqual(recipeInputs);
            })
            .ToArray();
        if (_availableRecipes.Length > 0)
        {
            SelectRecipe(_availableRecipes[0]);
        }
        else
        {
            ClearRecipe();
        }
    }

    private void SelectRecipe(AssemblyRecipeSo recipe)
    {
        _selectedRecipe = recipe;
        _hitToProcess = recipe.hitToProcess;
        OnRecipeSelected?.Invoke(this, new RecipeSelectedEventArgs(recipe.output, _availableRecipes.Length));
    }

    private void ClearRecipe()
    {
        _selectedRecipe = null;
        OnRecipeSelected?.Invoke(this, new RecipeSelectedEventArgs(null, 0));
    }

    public void InteractAlt()
    {
        if(_selectedRecipe is null) return;
        _state = State.Processing;
        _hitToProcess--;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = 1 - (float)_hitToProcess / _selectedRecipe.hitToProcess
        });
        
        if (_hitToProcess <= 0)
        {
            Transform();
        }
        
    }
    
    private void Transform()
    {
        _items.ToList().ForEach(i => i.DestroySelf());
        Item.SpawnItem<FinalProduct>(_selectedRecipe.output.prefab, this);
        ClearRecipe();
        _state = State.Idle;
    }
    
    public void InteractPrevious()
    {
        if(_state is State.Processing) return;
        if (_selectedRecipe is null) return;
        
        int index = Array.IndexOf(_availableRecipes, _selectedRecipe);
        index--;
        if (index < 0) index = _availableRecipes.Length - 1;
        SelectRecipe(_availableRecipes[index]);
    }
    
    public void InteractNext()
    {
        if(_state is State.Processing) return;
        if (_selectedRecipe is null) return;
        
        int index = Array.IndexOf(_availableRecipes, _selectedRecipe);
        index++;
        if (index >= _availableRecipes.Length) index = 0;
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

    public void AddItem<T>(Item newItem) where T : Item
    {
        if (typeof(T) == typeof(Product))
        {
            _items.Push(newItem as Product);
            return;
        }
        else if (typeof(T) == typeof(FinalProduct))
        {
            _finalProduct = newItem as FinalProduct;
            return;
        }
    
        Debug.LogWarning("This station can only hold products or final product!");
    }

    public Item[] GetItems<T>() where T : Item
    {
        if (typeof(T) != typeof(Product)) return null;
    
        return _items.Cast<Item>().ToArray();
    }

    public void ClearItem(Item itemToClear)
    {
        if (itemToClear is FinalProduct)
        {
            _finalProduct = null;
            return;
        }
        
        _items.Remove(itemToClear as Product);
    }

    public bool HaveItems<T>() where T : Item
    {
        if (typeof(T) != typeof(Product)) return false;
    
        return _items.Count > 0;
    }

    public bool HaveAnyItems()
    {
        return _items.Count > 0;
    }

    public Transform GetAvailableItemSlot<T>() where T : Item
    {
        if (typeof(T) == typeof(Product))
        {
            return itemSlots[_items.Count];
        }
        if (typeof(T) == typeof(FinalProduct))
        {
            return finalProductSlot;
        }
    
        Debug.LogWarning("This station can only hold products or final product!");
            return null;
    }

    public bool HasAvailableSlot<T>() where T : Item
    {
        if (typeof(T) != typeof(Product) && typeof(T) != typeof(FinalProduct)) return false;
    
        return _items.Count < _capacity;
    }
}
