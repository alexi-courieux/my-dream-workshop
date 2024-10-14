using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance { get; private set; }
    
    [SerializeField] private RecipeListSo initialRecipes;
    
    private List<RecipeSo> recipes;

    private void Awake()
    {
        Instance = this;
        recipes = new List<RecipeSo>(initialRecipes.recipes);
    }
    
    public void AddRecipe(RecipeSo recipeSo)
    {
        if(recipes.Contains(recipeSo)) return;
        
        recipes.Add(recipeSo);
    }
    
    public T[] GetRecipes<T>() where T : RecipeSo
    {
        List<T> recipesOfType = new List<T>();
        
        foreach (RecipeSo recipe in recipes)
        {
            if (recipe is T recipeOfType)
            {
                recipesOfType.Add(recipeOfType);
            }
        }

        return recipesOfType.ToArray();
    }
    
    public RecipeSo[] GetRecipes()
    {
        return recipes.ToArray();
    }
}