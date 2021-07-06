using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SSD_Assignment_1.Models
{
    public class MaximumAgeAttribute : ValidationAttribute
    {
        int MaximumAge;

        public MaximumAgeAttribute(int minimumAge)
        {
            MaximumAge = minimumAge;
            ErrorMessage = "You cannot enter an age of more than 100 years.";
        }

        public override bool IsValid(object value)
        {
            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date))
            {
                return date.AddYears(MaximumAge) > DateTime.Now;
            }
            return false;
        }
    }
}
