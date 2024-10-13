using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderMultipleRecipeUI : MonoBehaviour
{
    [SerializeField] private Transform recipesParent;
    [SerializeField] private GameObject recipeTemplate;
    [SerializeField] private TextMeshProUGUI groupNameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private OrderRecipeUI orderRecipeUI;

    private BuyableRecipeGroupSo recipeGroup;

    private void Awake()
    {
        buyButton.onClick.AddListener(() =>
        {
            OrderManager.Instance.BuyRecipeGroup(recipeGroup);
            orderRecipeUI.UpdateVisuals();
        });
    }

    private void Start()
    {
        recipeTemplate.SetActive(false);
    }

    public void UpdateVisual(BuyableRecipeGroupSo newRecipeGroupSo)
    {
        recipeGroup = newRecipeGroupSo;
        groupNameText.text = newRecipeGroupSo.groupName;
        priceText.text = newRecipeGroupSo.price.ToString("D");
        
        foreach (Transform child in recipesParent)
        {
            if (child == recipeTemplate.transform) continue;
            Destroy(child.gameObject);
        }
        
        foreach (RecipeSo recipe in recipeGroup.recipes)
        {
            GameObject orderSingleRecipeUI = Instantiate(recipeTemplate, recipesParent);
            orderSingleRecipeUI.SetActive(true);
            orderSingleRecipeUI.GetComponent<OrderMultipleRecipeSingleUI>().UpdateVisual(recipe);
        }
    }
    
    public void Select()
    {
        buyButton.Select();
    }
}