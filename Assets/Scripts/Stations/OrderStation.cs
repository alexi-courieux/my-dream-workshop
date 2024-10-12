using UnityEngine;

public class OrderStation : MonoBehaviour, IInteractable
{
    [SerializeField] private OrderUI orderUi;
    public void Interact()
    {
        orderUi.Show();
    }
}