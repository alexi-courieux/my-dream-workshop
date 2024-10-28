using System;

public interface ISelectableProduct
{
    public event EventHandler<SelectedProductEventArgs> OnProductSelected;
}

public class SelectedProductEventArgs : EventArgs
{
    public ProductSo Product { get; }
    public int AvailableProductsCount { get; }

    public SelectedProductEventArgs(ProductSo product, int availableProductsCount)
    {
        Product = product;
        AvailableProductsCount = availableProductsCount;
    }
}