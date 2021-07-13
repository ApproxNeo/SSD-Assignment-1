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
using Stripe;

namespace SSD_Assignment_1.Pages.Cart
{
    [AllowAnonymous] 
    public class CheckoutModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Data.SSD_Assignment_1Context _context;

        public CheckoutModel(ILogger<IndexModel> logger, Data.SSD_Assignment_1Context context)
        {
            _logger = logger;
            _context = context;
        }
        public IQueryable<SSD_Assignment_1.Models.CartItem> CartQuery;
        public IList<CartItem> Carts;
        public List<SSD_Assignment_1.Models.Product> Products;

        public string Secret { get; set; }


        public async Task OnGetAsync()
        {
            
            string UserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            CartQuery = from m in _context.CartItems where UserId.Equals(m.UserId) select m;

            var optionsCust = new CustomerCreateOptions { };

            var serviceCust = new CustomerService();
            var customer = serviceCust.Create(optionsCust);


            var optionsPay = new PaymentIntentCreateOptions
            {
                Amount = 1099,
                Currency = "sgd",
                Customer = customer.Id
            };

            var servicePay = new PaymentIntentService();
            var paymentIntent = servicePay.Create(optionsPay);

            Secret = paymentIntent.ClientSecret;
            

        }
        

    }
}
