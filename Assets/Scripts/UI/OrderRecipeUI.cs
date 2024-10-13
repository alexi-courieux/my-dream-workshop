using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrderRecipeUI : MonoBehaviour
{
    [SerializeField] private Transform orderItemsParent;
    [SerializeField] private GameObject orderSingleRecipeUITemplate;
    [SerializeField] private GameObject orderMultipleRecipeUITemplate;

    private void Awake()
    {
        orderSingleRecipeUITemplate.SetActive(false);
        orderMultipleRecipeUITemplate.SetActive(false);
    }

    private void Start()
    {
        UpdateVisuals();
    }

    public void OnEnable()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (OrderManager.Instance is null) return;
        RecipeSo[] buyableRecipes = OrderManager.Instance.GetBuyableRecipes();
        BuyableRecipeGroupSo[] buyableRecipeGroups = OrderManager.Instance.GetBuyableRecipeGroups();
        
        foreach (Transform child in orderItemsParent)
        {
            if (child == orderSingleRecipeUITemplate.transform
                || child == orderMultipleRecipeUITemplate.transform) continue;
            Destroy(child.gameObject);
        }
        
        foreach (RecipeSo recipe in buyableRecipes)
        {
            GameObject orderSingleRecipeUI = Instantiate(orderSingleRecipeUITemplate, orderItemsParent);
            orderSingleRecipeUI.SetActive(true);
            orderSingleRecipeUI.GetComponent<OrderSingleRecipeUI>().UpdateVisual(recipe);
        }
        
        foreach (BuyableRecipeGroupSo recipeGroup in buyableRecipeGroups)
        {
            GameObject orderMultipleRecipeUI = Instantiate(orderMultipleRecipeUITemplate, orderItemsParent);
            orderMultipleRecipeUI.SetActive(true);
            orderMultipleRecipeUI.GetComponent<OrderMultipleRecipeUI>().UpdateVisual(recipeGroup);
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
