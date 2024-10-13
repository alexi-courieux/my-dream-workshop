using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrderProductUI : MonoBehaviour, ITabElement
{
    [SerializeField] private Transform orderItemsParent;
    [SerializeField] private GameObject orderSingleItemUITemplate;

    private void Awake()
    {
        orderSingleItemUITemplate.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpdateVisuals();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void UpdateVisuals()
    {
        ProductSo[] buyableProducts = OrderManager.Instance.GetBuyableProducts();
        
        foreach (Transform child in orderItemsParent)
        {
            if (child == orderSingleItemUITemplate.transform) continue;
            Destroy(child.gameObject);
        }
        
        bool isFirst = true;
        foreach (ProductSo product in buyableProducts)
        {
            GameObject orderSingleItemUI = Instantiate(orderSingleItemUITemplate, orderItemsParent);
            orderSingleItemUI.SetActive(true);
            OrderSingleProductUI ui = orderSingleItemUI.GetComponent<OrderSingleProductUI>();
            ui.UpdateVisual(product);
            if (isFirst)
            {
                isFirst = false;
                StartCoroutine(ActionAfterDelay(() => ui.Select(), 0.1f));
            }
        }
        EventSystem.current.SetSelectedGameObject(null);
    }
    
    private IEnumerator ActionAfterDelay(Action action, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        action.Invoke();
    }
}
