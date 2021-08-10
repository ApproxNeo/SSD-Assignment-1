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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Stripe;

namespace SSD_Assignment_1.Pages.Cart
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Data.SSD_Assignment_1Context _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly INotyfService _notyf;


        public IndexModel(ILogger<IndexModel> logger, Data.SSD_Assignment_1Context context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, INotyfService notyf)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _notyf = notyf;
        }
        public IQueryable<SSD_Assignment_1.Models.CartItem> CartQuery;
        public IList<CartItem> Carts;
        public List<Models.Product> Products;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!(_signInManager.IsSignedIn(User)))
            {
                _notyf.Information("Register an account to start shopping with us!");
                return Redirect("/Identity/Account/Register");
            }

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            CartQuery = from m in _context.CartItems where UserId.Equals(m.UserId) select m;

            Carts = CartQuery.ToList();

            Products = new List<Models.Product>();
            foreach (var c in Carts)
            {
                Models.Product p = await _context.Product.FindAsync(c.ProductId);
                Products.Add(p);
            }

            return Page();

        }
        public class QChange
        {
            [Required]
            public int ProductId { get; set; }
            [Required]
            public int NQuantity { get; set; }
        }

        [BindProperty]
        public QChange QtyChange { get; set; }

        public async Task<IActionResult> OnPostQtyChangeAsync()
        {
            
            string UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            IQueryable<CartItem> CartQuery = from m in _context.CartItems where UserId.Equals(m.UserId) select m;
            IQueryable<CartItem> ModifiedCart = CartQuery.Where(s => s.ProductId == QtyChange.ProductId);


            if (QtyChange.NQuantity < 1 || QtyChange.NQuantity > 99 || ModifiedCart.Count() != 1)
            {
                _notyf.Information("Quantity can only be between 1 and 99 inclusive");
            }
            else
            {
                if (ModifiedCart.FirstOrDefault().UserId != UserId)
                {
                    return Redirect("~/");
                }
                ModifiedCart.FirstOrDefault().Quantity = QtyChange.NQuantity;
                await _context.SaveChangesAsync();
            }

            Carts = CartQuery.ToList();

            Products = new List<Models.Product>();
            foreach (var c in Carts)
            {
                Models.Product p = await _context.Product.FindAsync(c.ProductId);
                Products.Add(p);
            }

            return Page();

        }

        public IList<CartItem> OrderItems;

        public Dictionary<string, int> OrderDetails;

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            string UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            IQueryable<CartItem> CartQuery = from m in _context.CartItems where UserId.Equals(m.UserId) select m;

            if (CartQuery.Count() <= 0)
            {
                return Redirect("~/Cart");
            }

            Carts = CartQuery.ToList();

            Products = new List<Models.Product>();
            foreach (var c in Carts)
            {
                Models.Product p = await _context.Product.FindAsync(c.ProductId);
                Products.Add(p);
            }

            var user = await _userManager.GetUserAsync(User);

            //Create Stripe Customer if not made before
            if (user.StripeId is null)
            {
                var optionsCust = new CustomerCreateOptions { };

                var serviceCust = new CustomerService();
                var customer = serviceCust.Create(optionsCust);
                user.StripeId = customer.Id;
                await _context.SaveChangesAsync();
            }

            

            OrderItems = CartQuery.ToList();
            OrderDetails = new Dictionary<string, int>();
            decimal Price = 0;
            foreach (var i in OrderItems)
            {
                Models.Product prod = await _context.Product.FindAsync(i.ProductId);
                Price += i.Quantity * prod.Price; 
                OrderDetails[prod.Name] = i.Quantity;
                CartItem item = await _context.CartItems.FindAsync(i.CartItemId);
                _context.CartItems.Remove(item);
            }

            //Register payment Intent
            var optionsPay = new PaymentIntentCreateOptions
            {
                Amount = Convert.ToInt32( Price * 100),
                Currency = "sgd",
                Customer = user.StripeId
            };

            var servicePay = new PaymentIntentService();
            var paymentIntent = servicePay.Create(optionsPay);


            Models.Order order = new Models.Order()
            {
                UserId = UserId,
                OrderDetails = JsonConvert.SerializeObject(OrderDetails),
                IntentId = paymentIntent.Id,
                Price = Price,
                DeliveryAddress = "",
                PaymentStatus = "Unpaid"
            };

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            return Redirect("~/Orders");

        }

        [BindProperty]
        public int CartId { get; set; }
        public async Task<IActionResult> OnPostRmvProdAsync()
        {

            string UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            CartItem Item = await _context.CartItems.FindAsync(CartId);

            if (Item.UserId != UserId)
            {
                return Redirect("~/"); ;
            }

            _context.Remove(Item);
            await _context.SaveChangesAsync();
            _notyf.Success("Product removed from cart successfully");
            await OnGetAsync();
            return Page();




        }
    }
}
