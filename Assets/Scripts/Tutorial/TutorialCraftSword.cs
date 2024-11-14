using UnityEngine;

public class TutorialCraftSword : TutorialStep
{
    [SerializeField] private AssemblyStation assemblyStation;
    [SerializeField] private ProductSo swordProductSo;
    
    public override void Initialise()
    {
        assemblyStation.OnProductCrafted += AssemblyStation_OnProductCrafted;
    }

    private void AssemblyStation_OnProductCrafted(object sender, ProductSo e)
    {
        if (e != swordProductSo) return;
        Complete();
    }
    
    private void OnDestroy()
    {
        assemblyStation.OnProductCrafted -= AssemblyStation_OnProductCrafted;
    }
}