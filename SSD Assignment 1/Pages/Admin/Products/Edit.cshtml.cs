using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;

namespace SSD_Assignment_1.Pages.Admin.Products
{
    [Authorize(Roles = "Product manager,Business Owner")]

    public class EditModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;
        private readonly INotyfService _notyf;


        public EditModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var oldProduct = await _context.Product.FindAsync(Product.Id);

            if (Photo == null)
            {
                Console.WriteLine("is null");
                Product.PhotoPath = oldProduct.PhotoPath;
            } else
            {
                Console.WriteLine("not null");
                Product.PhotoPath = GenerateName();
            }

            _context.Entry(oldProduct).CurrentValues.SetValues(Product);

            try
            {
                await _context.SaveAudit(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _notyf.Success("Product updated successfully!");
            return RedirectToPage("./Index");
        }

        private string GenerateName()
        {
            string uniqueName = null;
            if (Photo != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                uniqueName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }

            return uniqueName;
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
