using UnityEngine;

public class TutorialGetLog : TutorialStep
{
    [SerializeField] private OrderChestStation orderChestStation;
    [SerializeField] private ProductSo logProductSo;
    [SerializeField] private GameObject orderChestStationIndicator;
    
    private void Start()
    {
        gameObject.SetActive(true);
        orderChestStationIndicator.SetActive(false);
    }
    
    public override void Show()
    {
        orderChestStationIndicator.SetActive(true);
        tutorialUI.setTutorialText("Get one of the bought logs from the order chest near the window.");
        orderChestStation.OnProductTaken += OrderChestStation_OnProductTaken;
    }
    
    private void OrderChestStation_OnProductTaken(object sender, ProductSo e)
    {
        if (e != logProductSo) return;
        
        tutorialUI.CompleteTutorialStep();
        orderChestStationIndicator.SetActive(false);
        Destroy(this);
    }
    
    private void OnDestroy()
    {
        orderChestStation.OnProductTaken -= OrderChestStation_OnProductTaken;
    }
}