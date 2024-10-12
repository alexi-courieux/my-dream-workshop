using UnityEngine;
using UnityEngine.Serialization;
public abstract class RecipeSo : ScriptableObject
{
    public ProductSo output;
    [FormerlySerializedAs("price")] public int buyPrice;
}