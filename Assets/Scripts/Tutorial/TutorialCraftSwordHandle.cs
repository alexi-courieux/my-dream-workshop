using UnityEngine;

public class TutorialCraftSwordHandle : TutorialStep
{
    [SerializeField] private SculptingStation sculptingStation;
    [SerializeField] private ProductSo swordHandleProductSo;
    [SerializeField] private GameObject stationIndicator;
    [SerializeField] private BuyableRecipeGroupSo recipesAfterCompletion;
    
    public override void Show()
    {
        stationIndicator.SetActive(true);
        sculptingStation.OnProductCrafted += SculptingStationOnProductCraft;
    }

    public override void Hide()
    {
        stationIndicator.SetActive(false);
    }

    private void SculptingStationOnProductCraft(object sender, ProductSo e)
    {
        if (e != swordHandleProductSo) return;
     
        OrderManager.Instance.BuyRecipeGroup(recipesAfterCompletion);
        stationIndicator.SetActive(false);
        Complete();
    }
    
    private void OnDestroy()
    {
        sculptingStation.OnProductCrafted -= SculptingStationOnProductCraft;
    }
}