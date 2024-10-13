using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderCapacityUI : MonoBehaviour
{
    [SerializeField] private Transform orderItemsParent;
    [SerializeField] private GameObject orderSingleCapacityUITemplate;

    private void Awake()
    {
        orderSingleCapacityUITemplate.SetActive(false);
    }

    public void OnEnable()
    {
        if (OrderManager.Instance is null) return; //Quick fix for null reference exception
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        CapacitySo[] buyableCapacities = OrderManager.Instance.GetBuyableCapacities();
        
        foreach (Transform child in orderItemsParent)
        {
            if (child == orderSingleCapacityUITemplate.transform) continue;
            Destroy(child.gameObject);
        }
        
        foreach (CapacitySo capacity in buyableCapacities)
        {
            GameObject orderSingleItemUI = Instantiate(orderSingleCapacityUITemplate, orderItemsParent);
            orderSingleItemUI.SetActive(true);
            orderSingleItemUI.GetComponent<OrderSingleCapacityUI>().UpdateVisual(capacity);
        }
    }
}
