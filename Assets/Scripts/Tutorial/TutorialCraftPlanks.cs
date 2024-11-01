using UnityEngine;

public class TutorialCraftPlanks : TutorialStep
{
    [SerializeField] private WoodcuttingStation woodcuttingStation;
    [SerializeField] private ProductSo plankProductSo;
    [SerializeField] private GameObject woodcuttingStationIndicator;
    
    public override void Show()
    {
        woodcuttingStationIndicator.SetActive(true);
        woodcuttingStation.OnProductCrafted += WoodcuttingStation_OnProductCraft;
    }

    public override void Hide()
    {
        woodcuttingStationIndicator.SetActive(false);
    }

    private void WoodcuttingStation_OnProductCraft(object sender, ProductSo e)
    {
        if (e != plankProductSo) return;
     
        woodcuttingStationIndicator.SetActive(false);
        Complete();
    }
    
    private void OnDestroy()
    {
        woodcuttingStation.OnProductCrafted -= WoodcuttingStation_OnProductCraft;
    }
}