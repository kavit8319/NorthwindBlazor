using System.ComponentModel.DataAnnotations;
using Northwind.Interface.Server.Resources.Localization;

namespace Northwind.Interface.Server.AddModelRequiredAttribution
{
    public class ShipperReturnView:BaseModelView
    {
        [Required(ErrorMessageResourceName = "EnteredCompanyName", ErrorMessageResourceType = typeof(ShippersRes))]
        [MaxLength(34,ErrorMessageResourceName = "MaxCompanyNameLen34", ErrorMessageResourceType = typeof(ShippersRes))]
        public string CompanyName { get; set; }
        [Required(ErrorMessageResourceName = "EnteredPhoneNumber", ErrorMessageResourceType = typeof(ShippersRes))]
        [MaxLength(18, ErrorMessageResourceName = "MaxPhoneLength18", ErrorMessageResourceType = typeof(ShippersRes))]
        [RegularExpression("^\\+\\((\\d{3})\\)\\d{3}-\\d{2}-\\d{2}",ErrorMessageResourceName = "RegexPhone", ErrorMessageResourceType = typeof(ShippersRes))]
        public string Phone { get; set; }
    }
}
