using System;

public interface ISelectableRecipe
{
    public event EventHandler<RecipeSelectedEventArgs> OnRecipeSelected;
}

public class RecipeSelectedEventArgs : EventArgs
{
    public ProductSo Output { get; }
    public int AvailableRecipesCount { get; }

    public RecipeSelectedEventArgs(ProductSo output, int availableRecipesCount)
    {
        Output = output;
        AvailableRecipesCount = availableRecipesCount;
    }
}