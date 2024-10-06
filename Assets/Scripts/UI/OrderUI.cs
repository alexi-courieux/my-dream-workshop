using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Transform orderItemsParent;
    [SerializeField] private GameObject orderSingleItemUITemplate;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        orderSingleItemUITemplate.SetActive(false);
        closeButton.onClick.AddListener(Hide);
    }
    
    private void EconomyManager_OnMoneyChanged(object sender, EventArgs e)
    {
        SetMoneyText();
    }

    private void Start()
    {
        Hide();
        EconomyManager.Instance.OnMoneyChanged += EconomyManager_OnMoneyChanged;
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
        SetMoneyText();
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
    
    private void SetMoneyText()
    {
        moneyText.text = EconomyManager.Instance.GetMoney().ToString("D");
    }
}
