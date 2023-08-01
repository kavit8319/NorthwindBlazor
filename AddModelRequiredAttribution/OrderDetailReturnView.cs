using System.ComponentModel.DataAnnotations;
using Northwind.Interface.Server.Resources.Localization;

namespace Northwind.Interface.Server.AddModelRequiredAttribution
{
    public class OrderDetailReturnView:BaseModelView
    {
      
        public string ProductName { get; set; }
        [Required(ErrorMessageResourceName = "SelectProduct", ErrorMessageResourceType = typeof(OrderRes))]
        public int? ProductID { get; set; }

        public int? UnitsInStock { get; set; }

        [Required(ErrorMessageResourceName = "EnteredQuantity", ErrorMessageResourceType = typeof(OrderRes))]
        public int? Quantity { get; set; }
        [Required(ErrorMessageResourceName = "EnteredUnitPrice", ErrorMessageResourceType = typeof(OrderRes))]
       // [Range(minimum:1, maximum:0, ErrorMessage = "Entered valid number")]
        public double? UnitPrice { get; set; }
        public double? Discount { get; set; }
        public bool IsNew { get; set; }
        public double? TotalSumm { get; set; }
    }
}
