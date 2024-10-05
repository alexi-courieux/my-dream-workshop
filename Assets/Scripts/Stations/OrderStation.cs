using UnityEngine;

public class OrderStation : MonoBehaviour, IInteractable
{
    [SerializeField] private OrderUI orderUI;
    public void Interact()
    {
        orderUI.Show();
    }
}