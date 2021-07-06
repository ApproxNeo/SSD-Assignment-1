using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SSD_Assignment_1.Models
{
    public class MinimumAgeAttribute : ValidationAttribute

    {
        int MinimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            MinimumAge = minimumAge;
            ErrorMessage = "You must be at least 18 years old to register an account with us.";
        }

        public override bool IsValid(object value)
        {
            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date))
            {
                return date.AddYears(MinimumAge) < DateTime.Now;
            }
            return false;
        }
    }
}
