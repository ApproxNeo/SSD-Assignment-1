using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SSD_Assignment_1.Models;

namespace SSD_Assignment_1.Pages
{
    [AllowAnonymous] 
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INotyfService _notyf;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(ILogger<IndexModel> logger, INotyfService notyf, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _notyf = notyf;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {

            if (_signInManager.IsSignedIn(User)){
                ApplicationUser user = await _userManager.GetUserAsync(User);


                if (user.TwoFactorEnabled == false)
                {
                    _notyf.Information("Remember to enable 2fa to add an extra layer of security on your account!");
                }
            }
            

            return Page();
        }
    }
}
