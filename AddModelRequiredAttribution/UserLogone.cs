using Northwind.Interface.Server.Resources.Localization;
using System.ComponentModel.DataAnnotations;
using Northwind.Interface.Server.Components;

namespace Northwind.Interface.Server.AddModelRequiredAttribution;

public class UserLogoneModel:BaseModelView
{
    [Required(ErrorMessageResourceName = "UserAdm_Email_Required", ErrorMessageResourceType = typeof(UsersAdmRes), AllowEmptyStrings = false)]
    [EmailAddress(ErrorMessageResourceName = "UserAdm_Email_EmailAddress", ErrorMessageResourceType = typeof(UsersAdmRes))]
    [StringLength(maximumLength: 50, ErrorMessage = "Max length 50")]
    public string EmailAddress { get; set; }
    [Required(ErrorMessageResourceName = "UserAdm_Password_Required", ErrorMessageResourceType = typeof(UsersAdmRes), AllowEmptyStrings = false)]
    [CustomValidationUserPassword(MinLength = 6)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}