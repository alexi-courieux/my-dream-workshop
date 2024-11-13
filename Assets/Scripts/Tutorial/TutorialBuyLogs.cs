using UnityEngine;

public class TutorialBuyLogs : TutorialStep
{
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private ProductSo logProductSo;
    
    private int logsBought;
    
    public override void Initialise()
    {
        OrderManager.Instance.OnProductBuy += OrderManager_OnProductBuy;
    }
    
    private void OrderManager_OnProductBuy(object sender, ProductSo e)
    {
        if (e != logProductSo) return;
        
        logsBought++;
        if (logsBought < 3) return;
        
        Complete();
    }
    
    private void OnDestroy()
    {
        OrderManager.Instance.OnProductBuy -= OrderManager_OnProductBuy;
    }
}