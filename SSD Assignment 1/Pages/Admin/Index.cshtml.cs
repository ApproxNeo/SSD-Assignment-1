using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SSD_Assignment_1.Pages
{
    [Authorize(Roles = "Admin, Product manager")]
    public class AdminIndexModel : PageModel
    {
        private readonly ILogger<AdminIndexModel> _logger;

        public AdminIndexModel(ILogger<IndexModel> logger)
        {
            _logger = (ILogger<AdminIndexModel>)logger;
        }

        public void OnGet()
        {

        }
    }
}