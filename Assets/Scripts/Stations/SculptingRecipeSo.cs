using UnityEngine;

[CreateAssetMenu(fileName = "SculptingRecipe_new", menuName = "ScriptableObject/Recipe/Sculpting")]
public class SculptingRecipeSo : RecipeSo
{
    public ProductSo input;
    public int hitToProcess;

}
