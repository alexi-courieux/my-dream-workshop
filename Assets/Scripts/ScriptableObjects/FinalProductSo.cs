using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObject/FinalProduct")]
public class FinalProductSo : ProductSo
{
    public enum ItemType {
        Helmet,
        Chest,
        Pants,
        Boot,
        Weapon
    }
    public ItemType itemType;
}
