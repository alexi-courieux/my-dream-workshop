using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }
    
    [SerializeField] private ProductDictionarySo buyableProducts;
    [SerializeField] private List<ChestStation> chestStations;
    
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
        foreach (ChestStation chestStation in chestStations)
        {
            if (chestStation.GetProductSo() != productSo) continue;
            chestStation.AddProduct();
            return;
        }
    }
}