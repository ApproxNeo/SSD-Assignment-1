using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;
using SSD_Assignment_1.Pages.Admin.Audits;

namespace SSD_Assignment_1.Pages.Admin.Audits
{
    [Authorize(Roles = "Business Owner,Admin")]
    //couldnt get "Product manager" role to work at all,
    //thinking its cause its 2 words, "prodMngr" works fine for me,
    //need someone help see if they experience same issue - Clement
    public class IndexModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;

        [BindProperty]
        public string EMAIL { get; set; }

        [BindProperty]
        public string PASSWORD { get; set; }

        public IndexModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context)
        {
            _context = context;
        }

        public IList<Audit> Audit { get; set; }

        public async Task OnGetAsync()
        {
            Audit = await _context.AuditLogs.ToListAsync();
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

        public string DictParser (string dict)
        {
            if (dict is null)
            {
                return "None";
            }
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(dict);
            string output = "";
            foreach (var value in dic)
            {
                output += value.Key + " : " + value.Value + "\n";
            }
            System.Console.WriteLine(output);
            return output;
        }
    }
}
