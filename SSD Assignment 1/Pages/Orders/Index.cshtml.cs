using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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


        public IndexModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Models.Order> Order { get; set; }

        public async Task OnGetAsync()
        {
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
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        [BindProperty]
        public string OrderId { get; set; }
    }
}
