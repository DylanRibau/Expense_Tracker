using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expense_Tracker.Data
{
    public class CustomRangeAttribute : ValidationAttribute
    {
        private int Min { get; set; }
        private string MaxField { get; set; }

        public CustomRangeAttribute(int min, string maxField)
        {
            Min = min;
            MaxField = maxField;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(MaxField);
            if (property == null)
            {
                return new ValidationResult("Unknown Property " + MaxField);
            }

            var maxValue = property.GetValue(validationContext.ObjectInstance, null);

            try
            {
                int num = Convert.ToInt32(value);
                int max = Convert.ToInt32(maxValue);

                if (num > max || num < Min)
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }
            } catch(Exception e)
            {
                return new ValidationResult("The value entered is too big.");
            }

            return null;
        }
    }
}
