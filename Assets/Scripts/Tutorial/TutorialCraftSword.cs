using UnityEngine;
using UnityEngine.Serialization;

public class TutorialCraftSword : TutorialStep
{
    [SerializeField] private GameObject stationIndicator;
    [SerializeField] private BuyableRecipeGroupSo recipesAfterCompletion;
    [SerializeField] private ProductSo[] orderablesAfterCompletion;
    
    public override void Show()
    {
        stationIndicator.SetActive(true);
        OrderManager.Instance.OnSell += OrderManagerOnSell;
    }

    public override void Hide()
    {
        stationIndicator.SetActive(false);
    }

    private void OrderManagerOnSell(object sender, System.EventArgs e)
    {
        OrderManager.Instance.BuyRecipeGroup(recipesAfterCompletion);
        foreach (ProductSo productSo in orderablesAfterCompletion)
        {
            OrderManager.Instance.AddBuyableProduct(productSo);
        }
        
        stationIndicator.SetActive(false);
        Complete();
    }
    
    private void OnDestroy()
    {
        OrderManager.Instance.OnSell -= OrderManagerOnSell;
    }
}