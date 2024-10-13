using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OrderSingleCapacityUI : MonoBehaviour
{
    [SerializeField] private Image capacityImage;
    [SerializeField] private TextMeshProUGUI capacityNameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private OrderCapacityUI orderCapacityUI;
    
    private CapacitySo capacitySo;

    private void Awake()
    {
        buyButton.onClick.AddListener(() =>
        {
            OrderManager.Instance.BuyCapacity(capacitySo);
            orderCapacityUI.UpdateVisuals();
        });
    }

    public void UpdateVisual(CapacitySo newCapacitySo)
    {
        capacitySo = newCapacitySo;
        capacityImage.sprite = newCapacitySo.icon;
        capacityNameText.text = newCapacitySo.capacityName;
        priceText.text = newCapacitySo.price.ToString("D");
    }
}