using System;
using UnityEngine;
using UnityEngine.UI;

namespace AshLight.BakerySim.UI
{
    public class RecipeSelectionUI : MonoBehaviour
    {
        [SerializeField] GameObject station;
        [SerializeField] Image icon;
        [SerializeField] private GameObject next;
        [SerializeField] private GameObject previous;
    
        private ProductSo _selectedOutput;
        private int _availableRecipesCount;

        private void Awake()
        {
            gameObject.SetActive(false);
            
            if (station.TryGetComponent(out IFocusable focusableStation))
            {
                focusableStation.OnFocus += FocusableStation_OnFocus;
                focusableStation.OnStopFocus += FocusableStation_OnStopFocus;
            }
            else
            {
                Debug.LogError("Station doesn't Implement IFocusable");
            }
            
            if (station.TryGetComponent(out ISelectableRecipe SelectableRecipeStation))
            {
                SelectableRecipeStation.OnRecipeSelected += SelectableRecipeStation_OnRecipeSelected;
            } else {
                Debug.LogError("Station doesn't Implement ISelectableRecipe");
            }
        }
    
        private void FocusableStation_OnFocus(object sender, EventArgs e)
        {
            Show();
        }
    
        private void FocusableStation_OnStopFocus(object sender, EventArgs e)
        {
            Hide();
        }
    
        private void SelectableRecipeStation_OnRecipeSelected(object sender, RecipeSelectedEventArgs e)
        {
            _selectedOutput = e.Output;
            _availableRecipesCount = e.AvailableRecipesCount;
            if (_selectedOutput is null)
            {
                Hide();
            }
            else
            {
                UpdateVisuals();
                Show();
            }
        }

        private void Show()
        {
            if (_selectedOutput is null) return;
            gameObject.SetActive(true);
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateVisuals()
        {
            icon.sprite = _selectedOutput.sprite;
            next.SetActive(_availableRecipesCount > 1);
            previous.SetActive(_availableRecipesCount > 1);
        }
    }
}
