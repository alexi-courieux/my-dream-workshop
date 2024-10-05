using UnityEngine;

[CreateAssetMenu(fileName = "MoldingRecipe_new", menuName = "ScriptableObject/Recipe/Molding")]
public class MoldingRecipeSo : ScriptableObject
{
    public ProductSo input;
    public ProductSo output;
    public float timeToProcess;
}
