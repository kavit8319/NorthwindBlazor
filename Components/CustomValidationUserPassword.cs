using Microsoft.Extensions.Localization;
using Northwind.Interface.Server.Resources.Localization;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Interface.Server.Components;

public class CustomValidationUserPassword:ValidationAttribute
{
    public int MinLength { get; set; }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext != null)
        {
            var strValue = value as string;
            
            var localizer = validationContext.GetService<IStringLocalizer<UsersAdmRes>>();
            if (!string.IsNullOrEmpty(strValue) && localizer != null)
            {
                if (strValue.Length < MinLength)
                    return new ValidationResult(ErrorMessage = localizer["MinLengthPassword6Symbols"]);
                if (!strValue.Any(el => Char.IsLetter(el)))
                    return new ValidationResult(ErrorMessage = localizer["StringMustContainOneLater"]);
                if (!strValue.Any(el => char.IsUpper(el)))
                    return new ValidationResult(ErrorMessage = localizer["StringMustContainUpperLater"]);

            }
        }
        return ValidationResult.Success;
    }
}