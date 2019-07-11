using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeltaXProject.CustomValidator
{
    public class CustomAgeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dateTime = (DateTime)value;
                if (dateTime.CompareTo(new DateTime(1902, 01, 01)) < 0)
                {
                    return new ValidationResult("Cannot be older than the oldest person alive.");
                }
                else if (dateTime.CompareTo(DateTime.Now.Date) > 0)
                {
                    return new ValidationResult("You cannot be born in the future");
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