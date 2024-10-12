using UnityEngine;
using UnityEngine.UI;

public class OrderMultipleRecipeSingleUI : MonoBehaviour
{
    [SerializeField] private Image productImage;
    
    public void UpdateVisual(RecipeSo newRecipeSo)
    {
        productImage.sprite = newRecipeSo.output.sprite;
    }
}