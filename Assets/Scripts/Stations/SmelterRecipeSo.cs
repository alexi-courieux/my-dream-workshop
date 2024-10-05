using UnityEngine;

[CreateAssetMenu(fileName = "SmelterRecipe_new", menuName = "ScriptableObject/Recipe/Smelter")]
public class SmelterRecipeSo : ScriptableObject
{
    public ProductSo input;
    public ProductSo output;
    public float timeToProcess;
}
