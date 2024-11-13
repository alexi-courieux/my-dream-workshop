using UnityEngine;
using UnityEngine.Serialization;

public class TutorialCraftSword : TutorialStep
{
    [SerializeField] private BuyableRecipeGroupSo recipesAfterCompletion;
    [SerializeField] private ProductSo[] orderablesAfterCompletion;
    
    public override void Initialise()
    {
        OrderManager.Instance.OnSell += OrderManagerOnSell;
    }

    private void OrderManagerOnSell(object sender, System.EventArgs e)
    {
        OrderManager.Instance.BuyRecipeGroup(recipesAfterCompletion);
        foreach (ProductSo productSo in orderablesAfterCompletion)
        {
            OrderManager.Instance.AddBuyableProduct(productSo);
        }
        
        Complete();
    }
    
    private void OnDestroy()
    {
        OrderManager.Instance.OnSell -= OrderManagerOnSell;
    }
}