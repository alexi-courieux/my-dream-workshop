using UnityEngine;

public class TutorialCraftSwordHandle : TutorialStep
{
    [SerializeField] private SculptingStation sculptingStation;
    [SerializeField] private ProductSo swordHandleProductSo;
    [SerializeField] private GameObject stationIndicator;
    [SerializeField] private BuyableRecipeGroupSo recipesAfterCompletion;
    
    private void Start()
    {
        gameObject.SetActive(true);
        stationIndicator.SetActive(false);
    }
    
    public override void Show()
    {
        tutorialUI.setTutorialText("A client came and is asking for a wood sword. Craft a sword handle at the sculpting station using the plank. Select the recipe with 'previous'(Z) and 'next'(X) and press 'Use'(F) until the process is completed.");
        stationIndicator.SetActive(true);
        sculptingStation.OnProductCrafted += SculptingStationOnProductCraft;
    }
    
    private void SculptingStationOnProductCraft(object sender, ProductSo e)
    {
        if (e != swordHandleProductSo) return;
     
        OrderManager.Instance.BuyRecipeGroup(recipesAfterCompletion);
        stationIndicator.SetActive(false);
        tutorialUI.CompleteTutorialStep();
        Destroy(this);
    }
    
    private void OnDestroy()
    {
        sculptingStation.OnProductCrafted -= SculptingStationOnProductCraft;
    }
}