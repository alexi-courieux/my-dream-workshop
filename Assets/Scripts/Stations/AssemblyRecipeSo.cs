using UnityEngine;

[CreateAssetMenu(fileName = "AssemblyRecipe_new", menuName = "ScriptableObject/Recipe/Assembly")]
public class AssemblyRecipeSo : ScriptableObject
{
    public ProductSo[] inputs;
    public FinalProductSo output;
    public int hitToProcess;
    public int buyPrice;
    public bool isUnlock;
}
