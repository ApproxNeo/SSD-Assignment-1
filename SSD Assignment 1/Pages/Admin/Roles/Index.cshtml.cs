using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSD_Assignment_1.Models;
using Microsoft.AspNetCore.Authorization;

namespace SSD_Assignment_1.Pages.Admin.Roles
{
    [Authorize(Roles = "Admin,Business Owner")]
    public class IndexModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        [BindProperty(SupportsGet = true)]
        public string Searchstring { get; set; }//Role name to type
       
        public IndexModel(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
     
        public List<ApplicationRole> ApplicationRole { get; set; }

        public async Task OnGetAsync()
        {
            var roles = from r in _roleManager.Roles
                        select r;
            if (!string.IsNullOrEmpty(Searchstring))
            {
                roles = roles.Where(s => s.Name.Contains(Searchstring));
            }

            
            // Get a list of roles
            ApplicationRole = await roles.ToListAsync();

        }

    }
}
