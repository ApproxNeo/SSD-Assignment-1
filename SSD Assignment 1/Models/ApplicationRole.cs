using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SSD_Assignment_1.Models
{
    public class ApplicationRole: IdentityRole
    {
        [Required]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter valid string.")]
        public string Description { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter valid string.")]
        public override string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IPAddress { get; set; }

    }
}
