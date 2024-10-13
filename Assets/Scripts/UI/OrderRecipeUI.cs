using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrderRecipeUI : MonoBehaviour, ITabElement
{
    [SerializeField] private Transform orderItemsParent;
    [SerializeField] private GameObject orderSingleRecipeUITemplate;
    [SerializeField] private GameObject orderMultipleRecipeUITemplate;

    private void Awake()
    {
        orderSingleRecipeUITemplate.SetActive(false);
        orderMultipleRecipeUITemplate.SetActive(false);
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
        RecipeSo[] buyableRecipes = OrderManager.Instance.GetBuyableRecipes();
        BuyableRecipeGroupSo[] buyableRecipeGroups = OrderManager.Instance.GetBuyableRecipeGroups();
        
        foreach (Transform child in orderItemsParent)
        {
            if (child == orderSingleRecipeUITemplate.transform
                || child == orderMultipleRecipeUITemplate.transform) continue;
            Destroy(child.gameObject);
        }
        
        bool isFirst = true;
        foreach (RecipeSo recipe in buyableRecipes)
        {
            GameObject orderSingleRecipeUI = Instantiate(orderSingleRecipeUITemplate, orderItemsParent);
            orderSingleRecipeUI.SetActive(true);
            OrderSingleRecipeUI ui = orderSingleRecipeUI.GetComponent<OrderSingleRecipeUI>();
            ui.UpdateVisual(recipe);
            if (isFirst)
            {
                isFirst = false;
                StartCoroutine(ActionAfterDelay(() => ui.Select(), 0.1f));
            }
        }
        
        foreach (BuyableRecipeGroupSo recipeGroup in buyableRecipeGroups)
        {
            GameObject orderMultipleRecipeUI = Instantiate(orderMultipleRecipeUITemplate, orderItemsParent);
            orderMultipleRecipeUI.SetActive(true);
            OrderMultipleRecipeUI ui = orderMultipleRecipeUI.GetComponent<OrderMultipleRecipeUI>();
            ui.UpdateVisual(recipeGroup);
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
