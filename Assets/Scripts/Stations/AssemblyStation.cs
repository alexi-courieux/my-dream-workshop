using UnityEngine;
using System;
using System.Linq;
using Utils;

public class AssemblyStation : MonoBehaviour, IInteractable, IInteractableAlt, IHandleItems, IHasProgress, ISelectableProduct, IInteractablePrevious, IInteractableNext, IFocusable
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

    [SerializeField] private Transform[] itemSlots;
    [SerializeField] private Transform finalProductSlot;
    
    private readonly StackList<Product> _items = new StackList<Product>();
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
        if (Player.Instance.HandleSystem.HaveAnyItemSelected())
        {
            if (!Player.Instance.HandleSystem.HaveItemSelected<Product>()) return;
            Item playerItem = Player.Instance.HandleSystem.GetSelectedItem();
            if (!HasAvailableSlot(playerItem)) return;
            
            playerItem.SetParent(this);

            CheckRecipes();
            _state = State.Idle;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
        }
        else
        {
            if (_items.Count <= 0) return;
            
            Item item = _items.Pop();
            item.SetParent(Player.Instance.HandleSystem);
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
            .OrderBy(i => i.id)
            .ToArray();
        _availableRecipes = RecipeManager.Instance.GetRecipes<AssemblyRecipeSo>();
        _availableRecipes = _availableRecipes
            .Where(r =>
            {
                ProductSo[] recipeInputs = r.inputs
                    .OrderBy(i => i.id)
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
        OnProductSelected?.Invoke(this, new SelectedProductEventArgs(recipe.output, _availableRecipes.Length));
    }

    private void ClearRecipe()
    {
        _selectedRecipe = null;
        OnProductSelected?.Invoke(this, new SelectedProductEventArgs(null, 0));
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
        Item.SpawnItem(_selectedRecipe.output.prefab, this);
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

    public void AddItem(Item newItem)
    {
        if (newItem is Product product)
        {
            _items.Push(product);
        }
    }

    public Item[] GetItems<T>() where T : Item
    {
        if (typeof(T) != typeof(Product)) return null;
    
        return _items.Cast<Item>().ToArray();
    }

    public void ClearItem(Item itemToClear)
    {
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

    public Transform GetAvailableItemSlot(Item newItem)
    {
        return newItem is Product ? itemSlots[_items.Count] : null;
    }

    public bool HasAvailableSlot(Item item)
    {
        return item switch
        {
            Product => _items.Count < _capacity,
            _ => false
        };
    }
}
