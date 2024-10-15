using UnityEngine;

public class TutorialCraftPlanks : TutorialStep
{
    [SerializeField] private WoodcuttingStation woodcuttingStation;
    [SerializeField] private ProductSo plankProductSo;
    [SerializeField] private GameObject woodcuttingStationIndicator;
    
    private void Start()
    {
        gameObject.SetActive(true);
        woodcuttingStationIndicator.SetActive(false);
    }
    
    public override void Show()
    {
        tutorialUI.setTutorialText("Craft a plank at the woodcutting station using one of the logs you bought. Put the log on the station (E) and press 'Use'(F) to start the crafting process");
        woodcuttingStationIndicator.SetActive(true);
        woodcuttingStation.OnProductCrafted += WoodcuttingStation_OnProductCraft;
    }
    
    private void WoodcuttingStation_OnProductCraft(object sender, ProductSo e)
    {
        if (e != plankProductSo) return;
     
        woodcuttingStationIndicator.SetActive(false);
        tutorialUI.CompleteTutorialStep();
        Destroy(this);
    }
    
    private void OnDestroy()
    {
        woodcuttingStation.OnProductCrafted -= WoodcuttingStation_OnProductCraft;
    }
}