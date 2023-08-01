using Northwind.Interface.Server.Resources.Localization;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Interface.Server.AddModelRequiredAttribution
{

    public class UserReturnView:UserLogoneModel
    {
        [Required(ErrorMessageResourceName = "UserAdm_FirstName_Required", ErrorMessageResourceType = typeof(UsersAdmRes), AllowEmptyStrings = false)]
        [StringLength(maximumLength: 50, ErrorMessage = "Max length 50")]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceName = "UserAdm_LastName_Required", ErrorMessageResourceType = typeof(UsersAdmRes), AllowEmptyStrings = false)]
        [StringLength(maximumLength: 50, ErrorMessage = "Max length 50")]
        public string LastName { get; set; }
        [Required(ErrorMessageResourceName = "UserAdm_RoleID_Required", ErrorMessageResourceType = typeof(UsersAdmRes))]
        [Range(minimum:1,int.MaxValue,ErrorMessageResourceName = "UserAdm_RoleID_Range", ErrorMessageResourceType = typeof(UsersAdmRes))]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
       
        [Compare("Password", ErrorMessageResourceType = typeof(UsersAdmRes), ErrorMessageResourceName = "UserAdm_PasswordComferm_Comparer")]
        public string PasswordConfirm { get; set; }
      
    }


}
