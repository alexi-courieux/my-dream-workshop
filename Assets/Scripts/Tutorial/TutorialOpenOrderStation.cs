using UnityEngine;

public class TutorialOpenOrderStation : TutorialStep
{
    [SerializeField] private OrderStation orderStation;
    
    public override void Initialise()
    {
        orderStation.OnInteract += OrderStation_OnInteract;
    }
    
    private void OnDestroy()
    {
        orderStation.OnInteract -= OrderStation_OnInteract;
    }
    
    private void OrderStation_OnInteract(object sender, System.EventArgs e)
    {
        Complete();
    }
}