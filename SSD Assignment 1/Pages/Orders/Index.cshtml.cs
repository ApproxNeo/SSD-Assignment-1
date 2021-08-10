using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;
using Stripe;

namespace SSD_Assignment_1.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;
        private readonly INotyfService _notyf;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public IndexModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context, SignInManager<ApplicationUser> signInManager, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
            _signInManager = signInManager;
        }

        [BindProperty]
        public List<Models.Order> Order { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            int Unpaid = 0;

            if (!(_signInManager.IsSignedIn(User)))
            {
                return Redirect("~/");
            }

            string UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            IQueryable<Models.Order> CartQuery = from m in _context.Order where UserId.Equals(m.UserId) select m;
            Order = await CartQuery.ToListAsync();

            foreach (var o in Order)
            {
                if (o.PaymentStatus == "Unpaid") {
                    var service = new PaymentIntentService();
                    var paymentIntent = service.Get(o.IntentId);

                    if (paymentIntent.Status == "succeeded")
                    {
                        o.PaymentStatus = paymentIntent.Status;
                        _notyf.Success("Payment successfully made");
                    }
                    else
                    {
                        Unpaid += 1;
                    }
                    
                }
            }

            await _context.SaveChangesAsync();

            if (Unpaid > 0)
            {
                _notyf.Information(String.Format("You have {0} order(s) with payment due", Unpaid));
            }

            return Page();
        }

        [BindProperty]
        public string OrderId { get; set; }
    }
}
