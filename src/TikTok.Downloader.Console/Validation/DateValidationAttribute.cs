using System.ComponentModel.DataAnnotations;

namespace TikTok.Downloader.Console.Validation
{
    internal sealed class DateValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return DateTimeOffset.TryParse(value.ToString(), out var _)
                ? ValidationResult.Success
                : new ValidationResult($"The field {validationContext.DisplayName} must be in format \"YYYY-MM-DD\", but provided {value}");
        }
    }
}
