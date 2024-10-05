using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObject/Product")]
public class ProductSo : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string itemName;
    public int buyPrice;
    public int sellPrice;

    public bool CanSell()
    {
        return sellPrice > 0f;
    }

    public bool CanBuy()
    {
        return buyPrice > 0f;
    }
}