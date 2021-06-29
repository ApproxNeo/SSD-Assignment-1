using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSD_Assignment_1.Models;
using Microsoft.AspNetCore.Authorization;
namespace SSD_Assignment_1.Pages.Admin.Roles
{
    [Authorize(Roles = "Admin")]
    public class ManageDeleteModel : PageModel
    {
        private readonly Data.SSD_Assignment_1Context _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ManageDeleteModel(Data.SSD_Assignment_1Context context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public SelectList RolesSelectList;
        //contain  a list of roles to populate select box
        public SelectList UsersSelectList;
        // contain  a list of Users to populate select box

        public string selectedrolename { set; get; }
        public string selectedusername { set; get; }
        public string delrolename { set; get; }
        public string delusername { set; get; }

        public int usercountinrole { set; get; }
        public IList<ApplicationRole> Listroles { get; set; }

        public async Task OnGetAsync()
        {   //HTTPGet  - when form is being loaded
            //get list of roles and users
            IQueryable<string> RoleQuery = from m in _roleManager.Roles orderby m.Name select m.Name;
            IQueryable<string> UsersQuery = from u in _context.Users orderby u.UserName select u.UserName;

            RolesSelectList = new SelectList(await RoleQuery.Distinct().ToListAsync());
            UsersSelectList = new SelectList(await UsersQuery.Distinct().ToListAsync());
            // Get all the roles 
            var roles = from r in _roleManager.Roles
                        select r;
            Listroles = await roles.ToListAsync();
        }



        public async Task<IActionResult> OnPostAsync(string delusername, string delrolename)
        {
            //When the Delete this user from  Role button is pressed 
            if (delusername == null || delrolename == null)
            {
                return RedirectToPage("Manage");
            }

            ApplicationUser user = _context.Users.Where(u => u.UserName == delusername).FirstOrDefault();

            if (await _userManager.IsInRoleAsync(user, delrolename))
            {
                await _userManager.RemoveFromRoleAsync(user, delrolename);

                TempData["message"] = "Role removed from this user successfully";
            }

            return RedirectToPage("Manage");
        }

    }
}
