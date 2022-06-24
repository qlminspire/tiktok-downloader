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

        return new ValidationResult($"Invalid value provided to parameter \"{validationContext.DisplayName}\". The path \"{value}\" is not exists.");
    }
}
