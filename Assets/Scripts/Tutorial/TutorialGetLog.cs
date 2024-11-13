using UnityEngine;

public class TutorialGetLog : TutorialStep
{
    [SerializeField] private OrderChestStation orderChestStation;
    [SerializeField] private ProductSo logProductSo;
    public override void Initialise()
    {
        orderChestStation.OnProductTaken += OrderChestStation_OnProductTaken;
    }

    private void OrderChestStation_OnProductTaken(object sender, ProductSo e)
    {
        if (e != logProductSo) return;

        Complete();
    }
    
    private void OnDestroy()
    {
        orderChestStation.OnProductTaken -= OrderChestStation_OnProductTaken;
    }
}