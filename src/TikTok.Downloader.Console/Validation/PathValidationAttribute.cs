using System.ComponentModel.DataAnnotations;

namespace TikTok.Downloader.Console.Validation;

internal sealed class PathValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string path && (File.Exists(path) || Directory.Exists(path)))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"The field {validationContext.DisplayName} must be existing file or folder. Provided path: \"{value}\"");
    }
}
