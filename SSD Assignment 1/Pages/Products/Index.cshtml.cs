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
//added to make a search functionality
using Microsoft.AspNetCore.Mvc.Rendering;

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
        //added for search functionality
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Genres { get; set; } //search by genre
        [BindProperty(SupportsGet = true)]
        public string SearchGenre { get; set; }
        public SelectList Brands { get; set; } //search by brand
        [BindProperty(SupportsGet = true)]
        public string SearchBrand { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<string> genreQuery = from m in _context.Product
                                            orderby m.Genre
                                            select m.Genre;
            IQueryable<string> brandQuery = from b in _context.Product
                                            orderby b.Brand
                                            select b.Brand;
            var products = from p in _context.Product
                             select p;
            if (!string.IsNullOrEmpty(SearchString))
            {
                products = _context.Product.Where(p => p.Name.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(SearchGenre))
            {
                products = products.Where(m => m.Genre == SearchGenre);
            }
            if (!string.IsNullOrEmpty(SearchBrand))
            {
                products = products.Where(b => b.Brand == SearchBrand);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Brands = new SelectList(await brandQuery.Distinct().ToListAsync());
            //above has been added for search functionality
            Product = await products.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(int? ProductId)
        {
            Product = await _context.Product.ToListAsync();

            if (!User.Identity.IsAuthenticated)
            {
                _notyf.Information("Register an account to start shopping with us!");
                return Redirect("/Identity/Account/Register");
            }

            string UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ProductId is null || UserId is null)
            {
                return Page();
            }

            IQueryable<CartItem> CartQuery = from m in _context.CartItems where UserId.Equals(m.UserId) select m;
            CartQuery = CartQuery.Where(s => s.ProductId == ProductId);

            if (CartQuery.Count() == 0)
            {
                _context.Add(new CartItem()
                {
                    ProductId = (int)ProductId,
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
