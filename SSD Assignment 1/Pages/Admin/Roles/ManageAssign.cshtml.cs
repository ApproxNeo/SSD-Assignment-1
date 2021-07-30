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
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Security.Claims;

namespace SSD_Assignment_1.Pages.Admin.Roles
{
    [Authorize(Roles = "Admin,Business Owner")]
    public class ManageAddModel : PageModel
    {
        private readonly Data.SSD_Assignment_1Context _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly INotyfService _notyf;

        public ManageAddModel(Data.SSD_Assignment_1Context context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, INotyfService notyf)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _notyf = notyf;
        }

        public SelectList RolesSelectList;
        //contain  a list of roles to populate select box
        public SelectList UsersSelectList;
        // contain  a list of Users to populate select box

        public string selectedrolename { set; get; }
        public string selectedusername { set; get; }

        public int usercountinrole { set; get; }
        public IList<ApplicationRole> Listroles { get; set; }


        public async Task OnGetAsync()
        {
            //HTTPGet  - before form is loaded
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


        public async Task<IActionResult> OnPostAsync(string selectedusername, string selectedrolename)
        {
            //When the Assign button is pressed 
            if (selectedusername == null || selectedrolename == null)
            {
                _notyf.Warning("Please choose a Role & User");
                return RedirectToPage("Manage");
            }

            ApplicationUser AppUser = _context.Users.SingleOrDefault(u => u.UserName == selectedusername);
            ApplicationRole AppRole = await _roleManager.FindByNameAsync(selectedrolename);

            IdentityResult roleResult = await _userManager.AddToRoleAsync(AppUser, AppRole.Name);

            if (roleResult.Succeeded)
            {

                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _notyf.Success("Role added successfully to this user!");

                // Login failed attempt - create an audit record
                var auditrecord = new AuditRecords()
                {
                    AuditActionType = "Assigned a role",
                    DateTimeStamp = DateTime.Now,
                    KeyAuditFieldID = selectedrolename,
                    PerformedOn = AppUser.Id,
                    PerformedBy = UserId,
                };
               
                _context.RoleAuditRecord.Add(auditrecord);
                await _context.SaveChangesAsync();

                return RedirectToPage("Manage");
            }

            _notyf.Warning("An error ocurred while adding that role");

            return RedirectToPage("Manage");
        }

       
    }
}
