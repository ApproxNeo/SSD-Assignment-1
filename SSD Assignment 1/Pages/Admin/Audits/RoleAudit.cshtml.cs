using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;

namespace SSD_Assignment_1.Pages.Admin.Audits.RoleAudit
{
    [Authorize(Roles = "Business Owner,Admin")]
    public class RoleAuditModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleAuditModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<AuditRecords> Audits { get; set; }

        public async Task OnGetAsync()
        {
            Audits = await _context.RoleAuditRecord.ToListAsync();
        }
    }
}
