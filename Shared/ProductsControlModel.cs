using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.Data;
using Northwind.Interface.Server.Pages;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;

namespace Northwind.Interface.Server.Shared
{
    public class ProductsControlModel<TLocal> : BaseComponentModel<TLocal>
    {
        protected CustomGridAddEditDel<ProductReturnView> productGrid;

        [Parameter] public bool ShowPageSize { get; set; } = false;
        [Parameter] public string ExcluseProductsIds { get; set; }

      

        protected string filterProductName { get; set; }
        protected string filterCategory { get; set; }
        protected string filterSupplier { get; set; }
        protected string filterQuantityPerUnit { get; set; }
        protected decimal filterUnitPrice { get; set; }
        protected short filterUnitInStock { get; set; }
        protected short filterUnitsOnOrder { get; set; }
        protected short filterReorderLevel { get; set; }
        protected bool filterDiscontinue { get; set; }

        public ProductReturnView SelectedProduct { get; set; }

        protected Query queryProducts { get; set; } = new Query();

       
        protected override void OnInitialized()
        {
            queryProducts.AddParams(Constans.ProductIds, ExcluseProductsIds);
            component.HeaderMessage = localResource["Title"];
            //HeaderMessage = "New Header Value";
        }
    
        public async Task OnActionBegin(ActionEventArgs<ProductReturnView> product)
        {
            try
            {
                if (product.RequestType == Syncfusion.Blazor.Grids.Action.Filtering && product.CurrentFilteringColumn != null)
                {
                    if (productGrid?.PageSettings.CurrentPage > 1)
                        await productGrid.CustomGoToPage(1);
                }
                if (product.RequestType == Syncfusion.Blazor.Grids.Action.Filtering && product.CurrentFilteringColumn == null)
                {
                    await productGrid.ClearFilteringAsync();
                }
                if (product.RequestType == Syncfusion.Blazor.Grids.Action.Filtering && productGrid != null)
                {
                    product.Cancel = true;
                    switch (product.CurrentFilteringColumn)
                    {
                        case nameof(ProductReturnView.ProductName):
                            await productGrid.CustomFilterByColumnAsync(product, filterProductName);
                            break;
                        case nameof(ProductReturnView.CategoryName):
                            await productGrid.CustomFilterByColumnAsync(product, filterCategory);
                            break;
                        case nameof(ProductReturnView.SupCompanyName):
                            await productGrid.CustomFilterByColumnAsync(product, filterSupplier);
                            break;
                        case nameof(ProductReturnView.QuantityPerUnit):
                            await productGrid.CustomFilterByColumnAsync(product, filterQuantityPerUnit);
                            break;
                        case nameof(ProductReturnView.UnitPrice):
                            if (filterUnitPrice > 0)
                                await productGrid.CustomFilterByColumnAsync(product, filterUnitPrice);
                            break;
                        case nameof(ProductReturnView.UnitsInStock):
                            if (filterUnitInStock > 0)
                                await productGrid.CustomFilterByColumnAsync(product, filterUnitInStock);
                            break;
                        case nameof(ProductReturnView.UnitsOnOrder):
                            if (filterUnitsOnOrder > 0)
                                await productGrid.CustomFilterByColumnAsync(product, filterUnitsOnOrder);
                            break;
                        case nameof(ProductReturnView.ReorderLevel):
                            if (filterReorderLevel > 0)
                                await productGrid.CustomFilterByColumnAsync(product, filterReorderLevel);
                            break;
                        case nameof(ProductReturnView.Discontinued):
                            await productGrid.CustomFilterByColumnAsync(product, filterDiscontinue);
                            break;
                    }
                }
                if (productGrid != null && productGrid.removeDialog != null)
                    if (product.RequestType == Syncfusion.Blazor.Grids.Action.Delete && productGrid.removeDialog.Flag)
                    {
                        product.Cancel = true;  //Cancel default delete action.
                        await productGrid.removeDialog.ShowRemoveMessage($" {product.Data.ProductName}");
                        productGrid.removeDialog.Flag = false;
                    }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ex.Message);
            }
        }

        protected void CustomRowSelecting(RowSelectingEventArgs<ProductReturnView> product)
        {
            SelectedProduct = product.Data;
        }
    }
}
