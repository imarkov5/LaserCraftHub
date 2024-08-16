using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaserCraftHub.Attributes
{
    public class AgeAndPastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || value is not DateTime date)
            {
                return new ValidationResult("Invalid date format.");
            }

            DateTime today = DateTime.Today;
            // Calculate the age based on the provided date
            int age = today.Year - date.Year;
            if (date >= today) // Check if the date is in the future
            {
                return new ValidationResult("Your birthday must be in the past.");
            }
            else if (date > today.AddYears(-age)) // Adjust for birthdays not yet occurred this year
            {
                age--;
            }
            // Check if the age is less than 18
            if (age < 18)
            {
                return new ValidationResult("You must be at least 18 years old.");
            }

            return ValidationResult.Success;
        }
    }
}