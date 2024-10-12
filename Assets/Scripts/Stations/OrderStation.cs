using UnityEngine;

public class OrderStation : MonoBehaviour, IInteractable
{
    //[SerializeField] private OrderUI orderUI;
    [SerializeField] private TabsManager tabsManager;
    public void Interact()
    {
        tabsManager.Show();
        //orderUI.Show();
    }
}