using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }
    
    [SerializeField] private ProductDictionarySo buyableProducts;
    [SerializeField] private ProductDictionarySo sellableProducts;
    [SerializeField] private OrderChestStation chestStation;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public ProductDictionarySo GetBuyableProducts()
    {
        return buyableProducts;
    }
    
    public ProductDictionarySo GetSellableProducts()
    {
        return sellableProducts;
    }
    
    public void Buy(ProductSo productSo)
    {
        if (EconomyManager.Instance.GetMoney() < productSo.buyPrice) return;
        
        EconomyManager.Instance.RemoveMoney(productSo.buyPrice);
        chestStation.AddProduct(productSo);
    }
    
    public void Sell(ProductSo productSo)
    {
        EconomyManager.Instance.AddMoney(productSo.sellPrice);
    }
}