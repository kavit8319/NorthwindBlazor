using Northwind.Interface.Server.Resources.Localization;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Interface.Server.AddModelRequiredAttribution
{
    public class EmployeeReturnView: BaseModelView
    {
        [Required(ErrorMessageResourceName = "EnteredLastName", ErrorMessageResourceType = typeof(EmployeesRes))]
       
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EmployeesRes), ErrorMessageResourceName = "EnteredFirstName")]

        public string FirstName { get; set; }
        public string TitleName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EmployeesRes), ErrorMessageResourceName = "EnteredTitle")]
        public int? TitleId { get; set; }
        public string TitleOfCourtesy { get; set; }

        [Required(ErrorMessageResourceType = typeof(EmployeesRes), ErrorMessageResourceName = "EnteredBirthDay",
            AllowEmptyStrings = false)]
        public DateTime? BirthDate { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(EmployeesRes), ErrorMessageResourceName = "EnteredHireDate", AllowEmptyStrings = false)]
        public DateTime? HireDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public byte[] Photo { get; set; }
        public string Notes { get; set; }
        [Required(ErrorMessageResourceType = typeof(EmployeesRes), ErrorMessageResourceName = "SelectedReportTo", AllowEmptyStrings = false)]
        public int? ReportsTo { get; set; } = null;

        public bool? isParent { get; set; }
        public int Lavel { get; set; }

      

    }

    





}
