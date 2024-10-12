using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderProductUI : MonoBehaviour
{
    [SerializeField] private Transform orderItemsParent;
    [SerializeField] private GameObject orderSingleItemUITemplate;

    private void Awake()
    {
        orderSingleItemUITemplate.SetActive(false);
    }

    private void Start()
    {
        UpdateVisuals();
    }

    public void OnEnable()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        ProductSo[] buyableProducts = OrderManager.Instance.GetBuyableProducts();
        
        foreach (Transform child in orderItemsParent)
        {
            if (child == orderSingleItemUITemplate.transform) continue;
            Destroy(child.gameObject);
        }
        
        foreach (ProductSo product in buyableProducts)
        {
            GameObject orderSingleItemUI = Instantiate(orderSingleItemUITemplate, orderItemsParent);
            orderSingleItemUI.SetActive(true);
            orderSingleItemUI.GetComponent<OrderSingleProductUI>().UpdateVisual(product);
        }
    }
}
