using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;

namespace SSD_Assignment_1.Pages.Admin.Products
{
    [Authorize(Roles = "Product manager")]

    public class CreateModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;
        private readonly INotyfService _notyf;


        public CreateModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _notyf.Success("Product added successfully!");
            _context.Product.Add(Product);
            await _context.SaveAudit(User?.FindFirst(ClaimTypes.NameIdentifier).Value);

            return RedirectToPage("./Index");
        }
    }
}
