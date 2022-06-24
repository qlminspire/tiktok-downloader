using System.ComponentModel.DataAnnotations;

namespace TikTok.Downloader.Console.Validation;

internal sealed class UrlValidationAttribute: ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if(Uri.TryCreate(value as string, UriKind.Absolute, out var _))
            return ValidationResult.Success;

        return new ValidationResult($"Invalid value provided to parameter \"{validationContext.DisplayName}\": \"{value}\"");
    }
}
