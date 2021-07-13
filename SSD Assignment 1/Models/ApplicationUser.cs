using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSD_Assignment_1.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string StripeId { get; set; }

    }
}
