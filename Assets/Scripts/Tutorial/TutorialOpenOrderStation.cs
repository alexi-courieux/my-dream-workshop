using UnityEngine;

public class TutorialOpenOrderStation : TutorialStep
{
    [SerializeField] private OrderStation orderStation;
    
    public override void Initialise()
    {
        orderStation.OnUse += OrderStation_OnInteract;
    }
    
    private void OnDestroy()
    {
        orderStation.OnUse -= OrderStation_OnInteract;
    }
    
    private void OrderStation_OnInteract(object sender, System.EventArgs e)
    {
        Complete();
    }
}