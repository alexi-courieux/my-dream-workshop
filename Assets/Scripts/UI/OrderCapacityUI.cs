using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrderCapacityUI : MonoBehaviour, ITabElement
{
    [SerializeField] private Transform orderItemsParent;
    [SerializeField] private GameObject orderSingleCapacityUITemplate;

    private void Awake()
    {
        orderSingleCapacityUITemplate.SetActive(false);
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

    public void UpdateVisuals()
    {
        CapacitySo[] buyableCapacities = OrderManager.Instance.GetBuyableCapacities();
        
        foreach (Transform child in orderItemsParent)
        {
            if (child == orderSingleCapacityUITemplate.transform) continue;
            Destroy(child.gameObject);
        }

        bool isFirst = true;
        foreach (CapacitySo capacity in buyableCapacities)
        {
            GameObject orderSingleItemUI = Instantiate(orderSingleCapacityUITemplate, orderItemsParent);
            orderSingleItemUI.SetActive(true);
            OrderSingleCapacityUI ui = orderSingleItemUI.GetComponent<OrderSingleCapacityUI>();
            ui.UpdateVisual(capacity);
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
