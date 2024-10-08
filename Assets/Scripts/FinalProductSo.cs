using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObject/FinalProduct")]
public class FinalProductSo : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string itemName;
    public int buyPrice;
    public int sellPrice;
    public enum ItemType {
        Helmet,
        Chest,
        Pants,
        Boot
    }
    public ItemType itemType;

    public override bool Equals(object other)
    {
        if (other is FinalProductSo finalProduct)
        {
            return finalProduct.itemName == itemName;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }
}
