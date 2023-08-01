using Northwind.Interface.Server.Resources.Localization;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Interface.Server.AddModelRequiredAttribution
{
    public class OrderReturnView:BaseModelView
    {
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string EmplFullName { get; set; }

        [Required(ErrorMessageResourceName = "EnteredFreight", ErrorMessageResourceType = typeof(OrderRes))]
        public double? Freight { get; set; }
        [Required(ErrorMessageResourceName = "EnterShipName", ErrorMessageResourceType = typeof(OrderRes))]
        public string ShipName { get; set; }
        [Required(ErrorMessageResourceName = "EnterShipAddress", ErrorMessageResourceType = typeof(OrderRes))]
        public string ShipAddress { get; set; }
        [Required(ErrorMessageResourceName = "SelectedShipCountry", ErrorMessageResourceType = typeof(OrderRes))]
        public string Country { get; set; }
        [Required( ErrorMessageResourceName = "SelectedShipCity", ErrorMessageResourceType = typeof(OrderRes))]
        public string City { get; set; }
        [Required(ErrorMessageResourceName = "SelectedPostalCode", ErrorMessageResourceType = typeof(OrderRes))]
        public string PostalCode { get; set; }

        public int CountPositions { get; set; }

        public int SummQuantity { get; set; }

        public double? TotalSumm { get; set; }

        [Required(ErrorMessageResourceName = "SelectedOrderDate", ErrorMessageResourceType = typeof(OrderRes))]
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }
        [Required(ErrorMessageResourceName = "SelectCustomer", ErrorMessageResourceType = typeof(OrderRes))]
        public int? CustomerId { get; set; }
        [Required(ErrorMessageResourceName = "SelectEmployee", ErrorMessageResourceType = typeof(OrderRes))]
        public int? EmployeeId { get; set; }
        [Required(ErrorMessageResourceName = "SelectShipVia", ErrorMessageResourceType = typeof(OrderRes))]
        public int? ShipVia { get; set; }

        public string ShipCompanyName { get; set; }
        public string ShipPhone { get; set; }
    }
}
