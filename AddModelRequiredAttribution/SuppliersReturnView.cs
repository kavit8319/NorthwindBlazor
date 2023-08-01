using Northwind.Interface.Server.Resources.Localization;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Interface.Server.AddModelRequiredAttribution
{
    public class SuppliersReturnView
    {
     
        public int? Id { get; set; }

        public string IdStr { get; set; }

        [Required(ErrorMessageResourceName = "Suppliers_Required_CompanyName", ErrorMessageResourceType = typeof(SuppliersRes), AllowEmptyStrings = false)]
        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public string Address { get; set; }

        
        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }
        public string CountryCode {get; set; }
       
       
        public string Country { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string HomePage { get; set; }

        public int? TotalRows { get; set; }
    }
}
