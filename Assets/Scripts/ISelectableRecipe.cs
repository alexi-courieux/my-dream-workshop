using System;

public interface ISelectableRecipe
{
    public event EventHandler<RecipeSelectedEventArgs> OnRecipeSelected;
}

public class RecipeSelectedEventArgs : EventArgs
{
    public ToolRecipeSo Recipe { get; }
    public int AvailableRecipesCount { get; }

    public RecipeSelectedEventArgs(ToolRecipeSo recipe, int availableRecipesCount)
    {
        Recipe = recipe;
        AvailableRecipesCount = availableRecipesCount;
    }
}