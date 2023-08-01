using Northwind.Interface.Server.Resources.Localization;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Interface.Server.AddModelRequiredAttribution
{
    public class CategoryReturnView
    {
        public int Id { get; set; }

        public string IdStr { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Entered_Category", ErrorMessageResourceType = typeof(CategoryRes))]
        public string CategoryName { get; set; } = default;
     
        public string Description { get; set; }
      
        public byte[] Picture { get; set; } 
      
        public int TotalRows { get; set; }
    }
}
