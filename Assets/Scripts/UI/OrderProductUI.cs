using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        if (OrderManager.Instance is null) return; //Quick fix for null reference exception
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
        
        foreach (Transform child in orderItemsParent)
        {
            if (child.gameObject.activeSelf)
            {
                child.GetComponentInChildren<Button>().Select(); // TODO Allow the gamepad navigation but trigger the button, caused by the use of DefaultInputActions in EventSystem ?
            }
        }
    }
}
