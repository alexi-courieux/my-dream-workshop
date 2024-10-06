using System;
using UnityEngine;
using UnityEngine.UI;

namespace AshLight.BakerySim.UI
{
    public class RecipeSelectionUI : MonoBehaviour
    {
        [SerializeField] GameObject station;
        [SerializeField] Image icon;
        [SerializeField] Image next;
        [SerializeField] Image previous;

        private static readonly Color Active = Color.black;
        private static readonly Color Inactive = Color.gray;
    
        private ToolRecipeSo _selectedRecipe;
        private int _availableRecipesCount;

        private void Awake()
        {
            gameObject.SetActive(false);
            
            if (station.TryGetComponent(out IFocusable focusableStation))
            {
                focusableStation.OnFocus += FocusableElementOnFocus;
                focusableStation.OnStopFocus += FocusableElementOnStopFocus;
            }
            else
            {
                Debug.LogError("Station doesn't Implement IFocusable");
            }
            
            if (station.TryGetComponent(out ISelectableRecipe SelectableRecipeStation))
            {
                SelectableRecipeStation.OnRecipeSelected += ClearStation_OnRecipeSelected;
            } else {
                Debug.LogError("Station doesn't Implement ISelectableRecipe");
            }
        }
    
        private void FocusableElementOnFocus(object sender, EventArgs e)
        {
            Show();
        }
    
        private void FocusableElementOnStopFocus(object sender, EventArgs e)
        {
            Hide();
        }
    
        private void ClearStation_OnRecipeSelected(object sender, RecipeSelectedEventArgs e)
        {
            _selectedRecipe = e.Recipe;
            _availableRecipesCount = e.AvailableRecipesCount;
            UpdateVisuals();
            Show();
        }

        private void Show()
        {
            if (_selectedRecipe is null) return;
            gameObject.SetActive(true);
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateVisuals()
        {
            if (_selectedRecipe is null)
            {
                Hide();
                return;
            }
            icon.sprite = _selectedRecipe.output.sprite;
            next.color = _availableRecipesCount > 1 ? Active : Inactive;
            previous.color = _availableRecipesCount > 1 ? Active : Inactive;
        }
    }
}
