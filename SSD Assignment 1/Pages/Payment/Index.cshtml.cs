using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;
using Stripe;

namespace SSD_Assignment_1.Pages.Payment
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            return Redirect("~/Orders");
        }

        [BindProperty]
        public int OrderId { get; set; }
        [BindProperty]
        public Models.Order order{ get; set; }
        [BindProperty]
        public string ClientSecret { get; set; }
        [BindProperty]
        public bool HasReuseable { get; set; }
        [BindProperty]
        public string Last4 { get; set; }
        [BindProperty]
        public string PUBLISHABLE_KEY { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            string UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.GetUserAsync(User);

            order = await _context.Order.FindAsync(OrderId);

            if (order.UserId != UserId)
            {
                return Redirect("~/Orders");
            }

            var service = new PaymentIntentService();
            var paymentIntent = service.Get(order.IntentId);

            if (paymentIntent.Status == "success")
            {
                return Redirect("~/Orders");
            }

            var options = new PaymentMethodListOptions
            {
                Customer = user.StripeId,
                Type = "card",
            };

            PUBLISHABLE_KEY = Environment.GetEnvironmentVariable("PUBLISHABLE_KEY");

            var methodsService = new PaymentMethodService();
            var paymentMethods = methodsService.List(options).ToList();
            if (paymentMethods.Count() == 0)
            {
                HasReuseable = false;
                ClientSecret = paymentIntent.ClientSecret;
                return Page();
            }
            Last4 = paymentMethods[0].Card.Last4;
            HasReuseable = true;
            return Page();
        }

        public async Task<IActionResult> OnPostReuseAsync()
        {
            string UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.GetUserAsync(User);

            Models.Order order = await _context.Order.FindAsync(OrderId);

            if (order.UserId != UserId)
            {
                return Redirect("~/Orders");
            }

            var service = new PaymentIntentService();
            var paymentIntent = service.Get(order.IntentId);

            if (paymentIntent.Status == "success")
            {
                return Redirect("~/Orders");
            }

            var options = new PaymentMethodListOptions
            {
                Customer = user.StripeId,
                Type = "card",
            };

            var methodsService = new PaymentMethodService();
            var paymentMethods = methodsService.List(options).ToList();
            if (paymentMethods.Count() == 0)
            {
                return Redirect("~/Orders");
            }

            var IntentOptions = new PaymentIntentUpdateOptions
            {
                PaymentMethod = paymentMethods[0].Id,
                
            };
               
            var IntentService = new PaymentIntentService();
            IntentService.Update(paymentIntent.Id, IntentOptions);
            IntentService.Confirm(paymentIntent.Id);

            return Redirect("~/Orders");





        }

    }
}
