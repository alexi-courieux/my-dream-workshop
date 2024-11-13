using UnityEngine;

public class TutorialCraftPlanks : TutorialStep
{
    [SerializeField] private WoodcuttingStation woodcuttingStation;
    [SerializeField] private ProductSo plankProductSo;
       
    public override void Initialise()
    {
        woodcuttingStation.OnProductCrafted += WoodcuttingStation_OnProductCraft;
    }

    private void WoodcuttingStation_OnProductCraft(object sender, ProductSo e)
    {
        if (e != plankProductSo) return;
     
        Complete();
    }
    
    private void OnDestroy()
    {
        woodcuttingStation.OnProductCrafted -= WoodcuttingStation_OnProductCraft;
    }
}