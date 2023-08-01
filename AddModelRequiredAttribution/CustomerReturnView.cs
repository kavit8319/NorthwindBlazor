using System.ComponentModel.DataAnnotations;
using Northwind.Interface.Server.Resources.Localization;

namespace Northwind.Interface.Server.AddModelRequiredAttribution
{
    public class CustomerReturnView:BaseModelView
    {
        [Required(ErrorMessageResourceName = "EnterСompanyName", ErrorMessageResourceType = typeof(CustomersRes))]
        public string CompanyName { get; set; }
        public string CustomerTitle { get; set; }

        [Required(ErrorMessageResourceName = "SelectCustomerTitle", ErrorMessageResourceType = typeof(CustomersRes))]
        public int? CustomerTitleId { get; set; }
        [Required(ErrorMessageResourceName = "EnterdContactName", ErrorMessageResourceType = typeof(CustomersRes))]
        public string ContactName { get; set; }
        [Required(ErrorMessageResourceName = "EnterdAddress", ErrorMessageResourceType = typeof(CustomersRes))]
        public string Address { get; set; }
        [Required(ErrorMessageResourceName = "SelectedCity", ErrorMessageResourceType = typeof(CustomersRes))]
        public string City { get; set; }
        [Required(ErrorMessageResourceName = "SelectedPostCode", ErrorMessageResourceType = typeof(CustomersRes))]
        public string PostalCode { get; set; }
        [Required(ErrorMessageResourceName = "SelectedCountry", ErrorMessageResourceType = typeof(CustomersRes))]
        public string Country { get; set; }
        public string Phone { get; set; }
        public bool? IsVip { get; set; }
    }
}
