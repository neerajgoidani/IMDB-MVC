using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeltaXProject.Models
{
    public class CustomDateValidator : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dateTime = (DateTime)value;
                if (dateTime.CompareTo(new DateTime(1888, 01, 01)) < 0)
                {
                    return new ValidationResult("Date cannot be before 1888 when the 1st movie was released");
                }
                else if (dateTime.CompareTo(new DateTime(2025, 01, 01)) > 0)
                {
                    return new ValidationResult("Cant add a movie for a year 2025 and ahead");
                }
                else
                {
                    return ValidationResult.Success;
                }

            }
            else
            {
                return new ValidationResult("" + validationContext.DisplayName + " is required");
            }
        }
    }
}