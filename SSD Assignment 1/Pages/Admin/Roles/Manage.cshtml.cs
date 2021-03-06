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
    [Authorize(Roles = "Admin,Business Owner")]
    public class ManageModel : PageModel
    {
        private readonly Data.SSD_Assignment_1Context _context;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ManageModel(Data.SSD_Assignment_1Context context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [BindProperty(SupportsGet = true)]
        public string Searchstring { get; set; }//Role name to type

        public SelectList RolesSelectList;
        //contain  a list of roles to populate select box
        public SelectList UsersSelectList;
        // contain  a list of Users to populate select box

        public int Usercountinrole { set; get; }
        public IList<ApplicationRole> Listroles { get; set; }

        public string ListUsersInRole(string rolename)
        {
            // Method - return a string showing a list of users  based on specified role as parameter
            string strListUsersInRole = "";
            string roleid = _roleManager.Roles.SingleOrDefault(u => u.Name == rolename).Id;

            // Get no. of users for each specified role
            var count = _context.UserRoles.Where(u => u.RoleId == roleid).Count();
            Usercountinrole = count;

            //Get a list of users for each specified role
            var listusers = _context.UserRoles.Where(u => u.RoleId == roleid);

            foreach (var oParam in listusers)
            {    // loop thru each objects-  get username based on userid and append to the returned string
                var userobj = _context.Users.SingleOrDefault(s => s.Id == oParam.UserId);
                strListUsersInRole += "[" + userobj.UserName + "] ";
            }
            return strListUsersInRole;
        }

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
            if (!string.IsNullOrEmpty(Searchstring))
            {
                roles = roles.Where(s => s.Name.Contains(Searchstring));
            }

            Listroles = await roles.ToListAsync();
        }

    }
}
