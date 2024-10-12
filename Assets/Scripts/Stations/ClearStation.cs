using System;
using System.Linq;
using UnityEngine;
public class ClearStation : MonoBehaviour, IInteractable, IHandleItems, IInteractableAlt, IFocusable, IInteractableNext, IInteractablePrevious, ISelectablProduct
{
    public event EventHandler OnFocus;
    public event EventHandler OnStopFocus;
    public event EventHandler<SelectedProductEventArgs> OnProductSelected;
    [SerializeField] private RecipesDictionarySo recipesDictionarySo;
    [SerializeField] private Transform productSlot;
    [SerializeField] private Transform toolSlot;
    private Product _product;
    private Tool _tool;
    private ToolRecipeSo[] _recipes;
    private ToolRecipeSo _selectedRecipe;

    public void Interact()
    {
        if (Player.Instance.HandleSystem.HaveAnyItems())
        {
            if (Player.Instance.HandleSystem.HaveItems<Product>())
            {
                if (HaveItems<Product>()) return;
                Player.Instance.HandleSystem.GetItem().SetParent(this);
            }
            else if (Player.Instance.HandleSystem.HaveItems<Tool>())
            {
                if (HaveItems<Tool>()) return;
                Player.Instance.HandleSystem.GetItem().SetParent(this);
            }
            else
            {
                Debug.LogWarning("Station can only hold products or tools!");
            }
        }
        else if (HaveAnyItems())
        {
            if (HaveItems<Product>())
            {
                _product.SetParent(Player.Instance.HandleSystem);
            } 
            else if (HaveItems<Tool>())
            {
                _tool.SetParent(Player.Instance.HandleSystem);
            }
        }
        RefreshRecipes();
    }

    public void InteractAlt()
    {
        if (!HaveItems<Product>()) return;
        if (!HaveItems<Tool>() && !Player.Instance.HandleSystem.HaveItems<Tool>()) return;
        if (_selectedRecipe is null) return;
        if(_product.ProductSo != _selectedRecipe.input) return;
        Tool tool = GetToolForRecipe();
        if(tool?.ToolSo != _selectedRecipe.tool) return;
    
        _product.DestroySelf();
        Item.SpawnItem(_selectedRecipe.output.prefab, this);
    }

    public void Focus()
    {
        RefreshRecipes();
        OnFocus?.Invoke(this, EventArgs.Empty);
    }

    private void RefreshRecipes()
    {
        Tool tool = GetToolForRecipe();

        if (tool is null || _product is null)
        {
            _recipes = null;
            _selectedRecipe = null;
            OnProductSelected?.Invoke(this, new SelectedProductEventArgs(null, 0));
        }
        else
        {
            _recipes = CheckForRecipes(tool.ToolSo, _product.ProductSo);
            _selectedRecipe = _recipes.FirstOrDefault();
            OnProductSelected?.Invoke(this, new SelectedProductEventArgs(_selectedRecipe?.output, _recipes.Length));
        }
    }

    private Tool GetToolForRecipe()
    {
        if(Player.Instance.HandleSystem.HaveItems<Tool>())
        {
            // We try to use tool holded by player in priority
            return Player.Instance.HandleSystem.GetItem() as Tool;
        }
        else
        {
            // If player doesn't hold any tool, we use the tool holded by the station
            return _tool;
        }
    }

    public void InteractNext()
    {
        if (_recipes is null || _recipes.Length == 0) return;
        int index = Array.IndexOf(_recipes, _selectedRecipe);
        index++;
        if (index >= _recipes.Length)
        {
            index = 0;
        }
        _selectedRecipe = _recipes[index];
        OnProductSelected?.Invoke(this, new SelectedProductEventArgs(_selectedRecipe?.output, _recipes.Length));
    }

    public void InteractPrevious()
    {
        if (_recipes is null || _recipes.Length == 0) return;
        int index = Array.IndexOf(_recipes, _selectedRecipe);
        index--;
        if (index < 0)
        {
            index = _recipes.Length - 1;
        }
        _selectedRecipe = _recipes[index];
        OnProductSelected?.Invoke(this, new SelectedProductEventArgs(_selectedRecipe?.output, _recipes.Length));
    }

    public void StopFocus()
    {
        OnStopFocus?.Invoke(this, EventArgs.Empty);
    }
    public ToolRecipeSo[] CheckForRecipes(ToolSo tool, ProductSo product)
    {
        return recipesDictionarySo.toolRecipes.Where(r => r.tool == tool && r.input == product).ToArray();
    }


    public void AddItem(Item newItem)
    {
        if (newItem is not Product && newItem is not Tool)
        {
            throw new Exception("This station can only hold products or tools!");
        }
    
        if (newItem is Product product)
        {
            _product = product;
        }
    
        if (newItem is Tool tool)
        {
            _tool = tool;
        }
    }

    public Item[] GetItems<T>() where T : Item
    {
        if (typeof(T) == typeof(Product))
        {
            return new Item[] { _product };
        }

        if (typeof(T) == typeof(Tool))
        {
            return new Item[] { _tool };
        }
        return new Item[] { };
    }

    public void ClearItem(Item itemToClear)
    {
        if (itemToClear is Product)
        {
            _product = null;
        }
        if (itemToClear is Tool)
        {
            _tool = null;
        }
    }

    public bool HaveItems<T>() where T : Item
    {
        if (typeof(T) == typeof(Product))
        {
            return _product is not null;
        }
    
        if (typeof(T) == typeof(Tool))
        {
            return _tool is not null;
        }
        return false;
    }

    public bool HaveAnyItems()
    {
        return _product is not null || _tool is not null;
    }

    public Transform GetAvailableItemSlot(Item newItem)
    {
        if (newItem is Product)
        {
            return productSlot;
        }

        if (newItem is Tool)
        {
            return toolSlot;
        }
        return null;
    }

    public bool HasAvailableSlot(Item item)
    {
        return item switch
        {
            Product => _product is null,
            Tool => _tool is null,
            _ => false
        };

    }
}