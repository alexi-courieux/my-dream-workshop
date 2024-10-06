using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Transform orderItemsParent;
    [SerializeField] private GameObject orderSingleItemUITemplate;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        orderSingleItemUITemplate.SetActive(false);
        Hide();
        
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        UpdateVisuals();
    }

    public void Show()
    {
        if (gameObject.activeSelf) return;
        InputManager.Instance.DisableGameplayInput();
        gameObject.SetActive(true);
        UpdateVisuals();
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
        InputManager.Instance.EnableGameplayInput();
    }

    private void UpdateVisuals()
    {
        ProductDictionarySo buyableProducts = OrderManager.Instance.GetBuyableProducts();
        
        foreach (Transform child in orderItemsParent)
        {
            if (child == orderSingleItemUITemplate.transform) continue;
            Destroy(child.gameObject);
        }
        
        foreach (ProductSo product in buyableProducts.products)
        {
            GameObject orderSingleItemUI = Instantiate(orderSingleItemUITemplate, orderItemsParent);
            orderSingleItemUI.SetActive(true);
            orderSingleItemUI.GetComponent<OrderSingleItemUI>().UpdateVisual(product);
        }
    }
}
