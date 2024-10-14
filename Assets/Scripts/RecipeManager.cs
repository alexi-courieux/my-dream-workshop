using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance { get; private set; }
    
    public event EventHandler OnStationUnlocked;
    
    [SerializeField] private RecipeListSo initialRecipes;
    [SerializeField] private RecipeBookStationSo[] initialUnlockedStationsSo;
    
    private List<RecipeSo> recipes;
    private List<RecipeBookStationSo> recipeBookStations;
    
    private void Awake()
    {
        Instance = this;
        recipes = new List<RecipeSo>(initialRecipes.recipes);
        recipeBookStations = new List<RecipeBookStationSo>(initialUnlockedStationsSo);
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
    
    public void AddRecipeBookStation(RecipeBookStationSo recipeBookStationSo)
    {
        if(recipeBookStations.Contains(recipeBookStationSo)) return;
        
        recipeBookStations.Add(recipeBookStationSo);
        OnStationUnlocked?.Invoke(this, EventArgs.Empty);
    }
    
    public RecipeBookStationSo[] GetRecipeBookStations()
    {
        return recipeBookStations.ToArray();
    }
}