using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSD_Assignment_1.Models;
using Microsoft.AspNetCore.Authorization;

namespace SSD_Assignment_1.Pages.Admin.Roles
{
    //[Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public CreateModel(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ApplicationRole ApplicationRole { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationRole.CreatedDate = DateTime.UtcNow;
            ApplicationRole.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            IdentityResult roleRuslt = await _roleManager.CreateAsync(ApplicationRole);

            return RedirectToPage("Index");
        }

    }
}
