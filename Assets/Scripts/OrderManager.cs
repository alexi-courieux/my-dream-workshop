using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }
    
    [SerializeField] private ProductDictionarySo buyableProducts;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public ProductDictionarySo GetBuyableProducts()
    {
        return buyableProducts;
    }
    
    public void Buy(ProductSo productSo)
    {
        Debug.Log($"Buying {productSo.name}");
    }
}