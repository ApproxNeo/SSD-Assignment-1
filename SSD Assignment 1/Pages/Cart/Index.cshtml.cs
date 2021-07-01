using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

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

        public void OnGet()
        {
            IQueryable<string> CartQuery = from m in _context.CartItems where (User.FindFirst(ClaimTypes.NameIdentifier).Value == m.UserID) select m.CartID;
        }
    }
}
