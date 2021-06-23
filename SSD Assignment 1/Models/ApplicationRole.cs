using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSD_Assignment_1.Models
{
    public class ApplicationRole: IdentityRole
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IPAddress { get; set; }

    }
}
