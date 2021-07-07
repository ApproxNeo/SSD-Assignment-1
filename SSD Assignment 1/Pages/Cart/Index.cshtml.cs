using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;
using SSD_Assignment_1.Pages.Cart;

namespace SSD_Assignment_1.Pages.Cart
{
    [AllowAnonymous] 
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Data.SSD_Assignment_1Context _context;

        public IndexModel(ILogger<IndexModel> logger, Data.SSD_Assignment_1Context context)
        {
            _logger = logger;
            _context = context;
        }
        public IQueryable<SSD_Assignment_1.Models.CartItem> CartQuery;
        public IList<CartItem> Carts;
        public List<Product> Products;

        public async Task OnGetAsync()
        {

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            CartQuery = from m in _context.CartItems where UserId.Equals(m.UserId) select m;

            Carts = CartQuery.ToList();

            Products = new List<Product>();
            foreach (var c in Carts)
            {
                int id = Convert.ToInt32(c.ProductId);
                Product p = await _context.Product.FindAsync(id);                
                Products.Add(p);
            }
            
        }
        public class qChange
        {
            [Required]
            public string ProductId { get; set; }
            [Required]
            public int NQuantity { get; set; }
        }

        [BindProperty]
        public qChange QChange { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {       
            string UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;          

            IQueryable<CartItem> CartQuery = from m in _context.CartItems where UserId.Equals(m.UserId) select m;
            IQueryable<CartItem> ModifiedCart = CartQuery.Where(s => s.ProductId == QChange.ProductId);
         
            if (ModifiedCart.Count() != 1)
            {
                return Page();
            }

            ModifiedCart.FirstOrDefault().Quantity = QChange.NQuantity;
            await _context.SaveChangesAsync();

            Carts = CartQuery.ToList();

            Products = new List<Product>();
            foreach (var c in Carts)
            {
                int id = Convert.ToInt32(c.ProductId);
                Product p = await _context.Product.FindAsync(id);
                Products.Add(p);
            }

            return Page();

        }

    }
}
