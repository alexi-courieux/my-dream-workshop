using UnityEngine;

public class TutorialOpenOrderStation : TutorialStep
{
    [SerializeField] private GameObject orderStationIndicator;
    
    [SerializeField] private OrderStation orderStation;
    
    public override void Show()
    {
        orderStation.OnInteract += OrderStation_OnInteract;
        orderStationIndicator.SetActive(true);
        tutorialUI.setTutorialText("Interact with the order station in your workshop to buy some logs");
    }
    
    public override void Hide()
    {
        orderStationIndicator.SetActive(false);
    }
    
    private void OnDestroy()
    {
        orderStation.OnInteract -= OrderStation_OnInteract;
    }
    
    private void OrderStation_OnInteract(object sender, System.EventArgs e)
    {
        orderStationIndicator.SetActive(false);
        tutorialUI.CompleteTutorialStep();
        Destroy(this);
    }
}