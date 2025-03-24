using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialSellSword : TutorialStep
{
    [SerializeField] private BuyableRecipeGroupSo recipesAfterCompletion;
    [SerializeField] private ProductSo[] orderablesAfterCompletion;
    [SerializeField] private GameObject[] stationsToUnlock;

    public override void Initialise()
    {
        OrderManager.Instance.OnSell += OrderManager_OnSell;
    }

    private void OrderManager_OnSell(object sender, System.EventArgs e)
    {
        OrderManager.Instance.BuyRecipeGroup(recipesAfterCompletion);
        foreach (ProductSo productSo in orderablesAfterCompletion)
        {
            OrderManager.Instance.AddBuyableProduct(productSo);
        }
        foreach (GameObject station in stationsToUnlock)
        {
            station.SetActive(true);
        }
        
        Complete();
    }
    
    private void OnDestroy()
    {
        OrderManager.Instance.OnSell -= OrderManager_OnSell;
    }
}