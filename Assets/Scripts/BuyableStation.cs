﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BuyableStation : MonoBehaviour, IUseable, IFocusable
{
    public event EventHandler OnFocus;
    public event EventHandler OnStopFocus;
    
    [SerializeField] private int price;
    [SerializeField] private GameObject[] stations;
    [SerializeField] private BuyableStation[] buyableStationsToDestroy;
    [SerializeField] private ProductSo[] orders;
    [SerializeField] private RecipeSo[] recipes;
    [SerializeField] private RecipeSo[] buyableRecipes;
    [SerializeField] private BuyableRecipeGroupSo[] buyableRecipeGroups;
    [SerializeField] private CapacitySo[] buyableCapacities;
    [SerializeField] private RecipeBookStationSo[] stationsToUnlockInRecipeBook;

    private void Start()
    {
        foreach (GameObject station in stations)
        {
            station.SetActive(false);
        }
    }
    
    public void Use()
    {
        if (EconomyManager.Instance.GetMoney() < price) return;
        
        EconomyManager.Instance.AddMoney(-price);
        foreach (GameObject station in stations)
        {
            station.SetActive(true);
        }
        foreach (ProductSo order in orders)
        {
            OrderManager.Instance.AddBuyableProduct(order);
        }
        foreach (RecipeSo recipe in buyableRecipes)
        {
            OrderManager.Instance.AddBuyableRecipe(recipe);
        }
        foreach (RecipeSo recipe in recipes)
        {
            RecipeManager.Instance.AddRecipe(recipe);
        }
        foreach (BuyableRecipeGroupSo recipeGroup in buyableRecipeGroups)
        {
            OrderManager.Instance.AddBuyableRecipeGroup(recipeGroup);
        }
        foreach (BuyableStation buyableStation in buyableStationsToDestroy)
        {
            buyableStation.DestroySelf();
        }
        foreach (CapacitySo capacity in buyableCapacities)
        {
            OrderManager.Instance.AddBuyableCapacity(capacity);
        }
        foreach (RecipeBookStationSo station in stationsToUnlockInRecipeBook)
        {
            RecipeManager.Instance.AddRecipeBookStation(station);
        }
        DestroySelf();
    }
    
    public void Focus()
    {
        OnFocus?.Invoke(this, EventArgs.Empty);
    }
    
    public void StopFocus()
    {
        OnStopFocus?.Invoke(this, EventArgs.Empty);
    }
    
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    
    public int GetPrice()
    {
        return price;
    }
}
