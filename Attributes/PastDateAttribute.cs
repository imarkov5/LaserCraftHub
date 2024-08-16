using System.ComponentModel.DataAnnotations;

namespace LaserCraftHub.Attributes;

public class PastDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return new ValidationResult("Please enter a date.");
        }

        if (value is not DateTime)
        {
            return new ValidationResult("Invalid date format.");
        }

        if ((DateTime)value > DateTime.Now)
        {
            return new ValidationResult("Date must be in the past.");
        }

        return ValidationResult.Success;
    }
}
