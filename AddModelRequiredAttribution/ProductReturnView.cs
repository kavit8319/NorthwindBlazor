using Northwind.Interface.Server.Resources.Localization;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Interface.Server.AddModelRequiredAttribution
{
    public class ProductReturnView:BaseModelView
    {
        [Required(ErrorMessageResourceName = "Entered_product_name", ErrorMessageResourceType = typeof(ProductsRes))]
        public string ProductName { get; set; }
        [Required(ErrorMessageResourceName = "Select_supplier", ErrorMessageResourceType = typeof(ProductsRes))]
        public int? SupplierId { get; set; }
        [Required(ErrorMessageResourceName ="Select_category", ErrorMessageResourceType =typeof(ProductsRes))]
        public int? CategoryId { get; set; }
        
        public string SupCompanyName { get; set; }
        
        public string CategoryName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        public string ExcludeProductIdStr { get; set; }
    }
}
