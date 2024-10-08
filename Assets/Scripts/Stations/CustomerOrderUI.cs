using UnityEngine;
using UnityEngine.UI;

public class CustomerOrderUI : MonoBehaviour
{
    [SerializeField] private Customer customer;
    [SerializeField] private Image productImage;
    
    private void Start()
    {
        customer.OnOrder += Customer_OnOrder;
        customer.OnLeave += Customer_OnLeave;
        Hide();
    }
    
    private void Customer_OnOrder(object sender, System.EventArgs e)
    {
        Show();
    }
    
    private void Customer_OnLeave(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Show()
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
        productImage.sprite = customer.GetOrder().sprite;
    }
    
}