using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;
namespace SSD_Assignment_1.Pages.Admin.Audits
{
    public class RoleAuditModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;

        public RoleAuditModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context)
        {
            _context = context;
        }

        public IList<AuditRecords> Audits { get; set; }

        public async Task OnGetAsync()
        {
            Audits = await _context.RoleAuditRecord.ToListAsync();
        }

        public string DictParsers(string dict)
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
