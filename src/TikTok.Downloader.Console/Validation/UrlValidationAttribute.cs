using System.ComponentModel.DataAnnotations;

namespace TikTok.Downloader.Console.Validation;

internal sealed class UrlValidationAttribute: ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        return Uri.TryCreate(value as string, UriKind.Absolute, out _) 
            ? ValidationResult.Success 
            : new ValidationResult($"The field {validationContext.DisplayName} must be valid url. Provided url: \"{value}\"");
    }
}
