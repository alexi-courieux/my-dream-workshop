using UnityEngine;

public class TutorialBuyLogs : TutorialStep
{
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private ProductSo logProductSo;
    
    private int logsBought;
    
    public override void Show()
    {
        tutorialUI.setTutorialText("Buy 3 logs.");
        OrderManager.Instance.OnProductBuy += OrderManager_OnProductBuy;
    }
    
    public override void Hide()
    {
        
    }
    
    private void OrderManager_OnProductBuy(object sender, ProductSo e)
    {
        if (e != logProductSo) return;
        
        logsBought++;
        if (logsBought < 3) return;
        
        tutorialUI.CompleteTutorialStep();
        Destroy(this);
    }
    
    private void OnDestroy()
    {
        OrderManager.Instance.OnProductBuy -= OrderManager_OnProductBuy;
    }
}