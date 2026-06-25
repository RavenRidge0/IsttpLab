using System;
using System.ComponentModel.DataAnnotations;

namespace DealershipDomain.Model
{
    public class CurrentYearMaxAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int year)
            {
                if (year > DateTime.Now.Year)
                {
                    return new ValidationResult(ErrorMessage ?? $"Рік випуску не може бути більшим за поточний ({DateTime.Now.Year})");
                }
            }
            return ValidationResult.Success;
        }
    }
}
