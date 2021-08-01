using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
    public class RoleAuditModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public string EMAIL { get; set; }

        [BindProperty]
        public string PASSWORD { get; set; }
        public RoleAuditModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<AuditRecords> Audits { get; set; }

        public async Task OnGetAsync()
        {
            Audits = await _context.RoleAuditRecord.ToListAsync();
            if (DateTime.Now.TimeOfDay == new TimeSpan(0, 0, 0))
            {
                EMAIL = Environment.GetEnvironmentVariable("EMAIL");
                PASSWORD = Environment.GetEnvironmentVariable("PASSWORD");
                string to = EMAIL; //To address    
                string from = EMAIL; //From address    
                MailMessage message = new MailMessage(from, to);

                string mailbody = "Dear Admin," +
                    "Update in roles are assigned.Please take a look at it.Thank you! ";
                message.Subject = "Update on role assigned";
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.outlook.com", 587); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(EMAIL, PASSWORD);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(message);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
