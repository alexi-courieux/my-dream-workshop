using UnityEngine;

public class TutorialCraftSwordHandle : TutorialStep
{
    [SerializeField] private SculptingStation sculptingStation;
    [SerializeField] private ProductSo swordHandleProductSo;
    [SerializeField] private BuyableRecipeGroupSo recipesAfterCompletion;
    
    public override void Initialise()
    {
        sculptingStation.OnProductCrafted += SculptingStationOnProductCraft;
    }

    private void SculptingStationOnProductCraft(object sender, ProductSo e)
    {
        if (e != swordHandleProductSo) return;
     
        OrderManager.Instance.BuyRecipeGroup(recipesAfterCompletion);
        Complete();
    }
    
    private void OnDestroy()
    {
        sculptingStation.OnProductCrafted -= SculptingStationOnProductCraft;
    }
}