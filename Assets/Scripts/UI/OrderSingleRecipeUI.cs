using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OrderSingleRecipeUI : MonoBehaviour
{
    [SerializeField] private Image productImage;
    [SerializeField] private TextMeshProUGUI productNameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private OrderRecipeUI orderRecipeUI;

    private RecipeSo recipeSo;

    private void Awake()
    {
        buyButton.onClick.AddListener(() =>
        {
            OrderManager.Instance.BuyRecipe(recipeSo);
            orderRecipeUI.UpdateVisuals();
        });
    }

    public void UpdateVisual(RecipeSo newRecipeSo)
    {
        this.recipeSo = newRecipeSo;
        productImage.sprite = newRecipeSo.output.sprite;
        productNameText.text = newRecipeSo.name;
        priceText.text = newRecipeSo.buyPrice.ToString("D");
    }
}