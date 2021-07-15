using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;

namespace SSD_Assignment_1.Pages.Admin.Orders
{
    [Authorize(Roles = "Public Relation,Business Owner")]
    public class DetailsModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;

        public DetailsModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Order.FirstOrDefaultAsync(m => m.OrderId == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
