using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }
    
    [SerializeField] private ProductDictionarySo initialBuyableProducts;
    [SerializeField] private ProductDictionarySo sellableProducts;
    [SerializeField] private OrderChestStation chestStation;
    
    private List<ProductSo> buyableProducts;
    private List<RecipeSo> buyableRecipes;
    private List<BuyableRecipeGroupSo> buyableRecipeGroups;
    
    private void Awake()
    {
        Instance = this;
        buyableProducts = initialBuyableProducts.products.ToList();
        buyableRecipes = new List<RecipeSo>();
        buyableRecipeGroups = new List<BuyableRecipeGroupSo>();
    }
    
    public ProductSo[] GetBuyableProducts()
    {
        return buyableProducts.ToArray();
    }
    
    public RecipeSo[] GetBuyableRecipes()
    {
        return buyableRecipes.ToArray();
    }
    
    public BuyableRecipeGroupSo[] GetBuyableRecipeGroups()
    {
        return buyableRecipeGroups.ToArray();
    }
    
    public void AddBuyableProduct(ProductSo productSo)
    {
        if(buyableProducts.Contains(productSo)) return;
        
        buyableProducts.Add(productSo);
    }
    
    public void AddBuyableRecipe(RecipeSo recipeSo)
    {
        if(buyableRecipes.Contains(recipeSo)) return;
        
        buyableRecipes.Add(recipeSo);
    }
    
    public void AddBuyableRecipeGroup(BuyableRecipeGroupSo buyableRecipeGroupSo)
    {
        if(buyableRecipeGroups.Contains(buyableRecipeGroupSo)) return;
        
        buyableRecipeGroups.Add(buyableRecipeGroupSo);
    }
    
    public ProductDictionarySo GetSellableProducts()
    {
        return sellableProducts;
    }
    
    public void BuyProduct(ProductSo productSo)
    {
        if (EconomyManager.Instance.GetMoney() < productSo.buyPrice) return;
        
        EconomyManager.Instance.RemoveMoney(productSo.buyPrice);
        chestStation.AddProduct(productSo);
    }
    
    public void BuyRecipe(RecipeSo recipeSo)
    {
        if (EconomyManager.Instance.GetMoney() < recipeSo.buyPrice) return;
        
        EconomyManager.Instance.RemoveMoney(recipeSo.buyPrice);
        RecipeManager.Instance.AddRecipe(recipeSo);
        buyableRecipes.Remove(recipeSo);
    }
    
    public void BuyRecipeGroup(BuyableRecipeGroupSo recipeGroupSo)
    {
        if (EconomyManager.Instance.GetMoney() < recipeGroupSo.price) return;
        
        EconomyManager.Instance.RemoveMoney(recipeGroupSo.price);
        foreach (RecipeSo recipe in recipeGroupSo.recipes)
        {
            RecipeManager.Instance.AddRecipe(recipe);
        }
        buyableRecipeGroups.Remove(recipeGroupSo);
    }
    
    public void Sell(ProductSo productSo)
    {
        EconomyManager.Instance.AddMoney(productSo.sellPrice);
    }
}