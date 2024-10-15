using UnityEngine;
using UnityEngine.Serialization;

public class TutorialCraftSword : TutorialStep
{
    [SerializeField] private GameObject stationIndicator;
    [SerializeField] private BuyableRecipeGroupSo recipesAfterCompletion;
    [SerializeField] private ProductSo[] orderablesAfterCompletion;
    
    private void Start()
    {
        gameObject.SetActive(true);
        stationIndicator.SetActive(false);
    }
    
    public override void Show()
    {
        tutorialUI.setTutorialText("Craft the sword at the assembly table and sell it. You can see the recipe in the recipe book (TAB).");
        stationIndicator.SetActive(true);
        OrderManager.Instance.OnSell += OrderManagerOnSell;
    }
    
    private void OrderManagerOnSell(object sender, System.EventArgs e)
    {
        OrderManager.Instance.BuyRecipeGroup(recipesAfterCompletion);
        foreach (ProductSo productSo in orderablesAfterCompletion)
        {
            OrderManager.Instance.AddBuyableProduct(productSo);
        }
        
        stationIndicator.SetActive(false);
        tutorialUI.CompleteTutorialStep();
        Destroy(this);
    }
    
    private void OnDestroy()
    {
        OrderManager.Instance.OnSell -= OrderManagerOnSell;
    }
}