using UnityEngine;

public class BuyableStation : MonoBehaviour, IInteractable
{
    [SerializeField] private int price;
    [SerializeField] private GameObject[] stations;
    [SerializeField] private ProductSo[] orders;
    [SerializeField] private RecipeSo[] recipes;
    [SerializeField] private RecipeSo[] buyableRecipes;
    [SerializeField] private BuyableRecipeGroupSo[] recipeGroups;

    private void Start()
    {
        foreach (GameObject station in stations)
        {
            station.SetActive(false);
        }
    }
    
    public void Interact()
    {
        if (EconomyManager.Instance.GetMoney() <= price) return;
        
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
        foreach (BuyableRecipeGroupSo recipeGroup in recipeGroups)
        {
            OrderManager.Instance.AddBuyableRecipeGroup(recipeGroup);
        }
        DestroySelf();
    }
    
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
