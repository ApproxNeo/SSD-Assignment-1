using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;
using SSD_Assignment_1.Pages.Cart;

namespace SSD_Assignment_1.Pages.Products
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;
        private readonly INotyfService _notyf;

        public IndexModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IList<Product> Product { get; set; }
        public CartItem CartItem { get; set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Product.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string ProductId)
        {
            Product = await _context.Product.ToListAsync();

            if (!User.Identity.IsAuthenticated)
            {
                _notyf.Information("Log in to access our services");
                return Redirect("/Identity/Account/Register");
            }

            string UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ProductId is null || UserId is null)
            {
                return Page();
            }

            IQueryable<SSD_Assignment_1.Models.CartItem> CartQuery = from m in _context.CartItems where UserId.Equals(m.UserId) select m;
            CartQuery = CartQuery.Where(s => s.ProductId == ProductId);

            if (CartQuery.Count() == 0)
            {
                _context.Add(new CartItem()
                {
                    ProductId = ProductId,
                    UserId = UserId,
                    Quantity = 1
                });
                await _context.SaveChangesAsync();
                _notyf.Success("Item added to cart!");
                return Page();
            }

            _notyf.Information("Item added to cart already");
            return Page();
        }
    }
}
