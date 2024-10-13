using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        
        foreach (Transform child in orderItemsParent)
        {
            if (child.gameObject.activeSelf)
            {
                child.GetComponentInChildren<Button>().Select(); // TODO Allow the gamepad navigation but triggr the button, caused by the use of DefaultInputActions in EventSystem ?
            }
        }
    }
}
