using Northwind.Interface.Server.Resources.Localization;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Interface.Server.AddModelRequiredAttribution
{
    public class FogotPasswordView:BaseModelView
    {
        [Required(ErrorMessageResourceName = "UserAdm_Email_Required", ErrorMessageResourceType = typeof(UsersAdmRes), AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessageResourceName = "UserAdm_Email_EmailAddress", ErrorMessageResourceType = typeof(UsersAdmRes))]
        [StringLength(maximumLength: 50, ErrorMessage = "Max length 50")]
        public string Email { get; set; }
    }
}
