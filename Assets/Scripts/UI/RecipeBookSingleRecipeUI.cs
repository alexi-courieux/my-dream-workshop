using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookSingleRecipeUI : MonoBehaviour
{
    [SerializeField] private RecipeBookSingleRecipeSingleInputUI inputTemplate;
    [SerializeField] private Image output;
    [SerializeField] private Transform inputsParent;
    
    private void Start()
    {
        inputTemplate.gameObject.SetActive(false);
    }

    public void UpdateVisuals(RecipeSo recipe)
    {
        foreach (Transform child in inputsParent)
        {
            if (child == inputTemplate.transform) continue;
            
            Destroy(child.gameObject);
        }

        var inputs = GetInputQuantities(recipe);
        
        foreach (var input in inputs)
        {
            RecipeBookSingleRecipeSingleInputUI inputUI = Instantiate(inputTemplate, inputsParent);
            inputUI.gameObject.SetActive(true);
            inputUI.UpdateVisuals(input.Key, input.Value);
        }
        
        output.sprite = recipe.output.sprite;
    }
    
    private Dictionary<ProductSo, int> GetInputQuantities(RecipeSo recipe)
    {
        Dictionary<ProductSo, int> inputQuantities = new Dictionary<ProductSo, int>();
        
        foreach (ProductSo input in recipe.inputs)
        {
            if (inputQuantities.ContainsKey(input))
            {
                inputQuantities[input]++;
            }
            else
            {
                inputQuantities.Add(input, 1);
            }
        }

        return inputQuantities;
    }
}