using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OrderSingleProductUI : MonoBehaviour
{
    [SerializeField] private Image productImage;
    [SerializeField] private TextMeshProUGUI productNameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;

    private ProductSo productSo;

    private void Awake()
    {
        buyButton.onClick.AddListener(() =>
        {
            OrderManager.Instance.BuyProduct(productSo);
        });
    }

    public void UpdateVisual(ProductSo newProductSo)
    {
        this.productSo = newProductSo;
        productImage.sprite = newProductSo.sprite;
        productNameText.text = newProductSo.name;
        priceText.text = newProductSo.buyPrice.ToString("D");
    }
    
    public void Select()
    {
        buyButton.Select();
    }
}