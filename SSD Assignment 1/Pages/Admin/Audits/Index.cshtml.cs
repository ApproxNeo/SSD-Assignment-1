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
    [Authorize(Roles = "Product manager,Admin")]
    //couldnt get "Product manager" role to work at all,
    //thinking its cause its 2 words, "prodMngr" works fine for me,
    //need someone help see if they experience same issue - Clement
    public class IndexModel : PageModel
    {
        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;

        public IndexModel(SSD_Assignment_1.Data.SSD_Assignment_1Context context)
        {
            _context = context;
        }

        public IList<Audit> Audit { get; set; }

        public async Task OnGetAsync()
        {
            Audit = await _context.AuditLogs.ToListAsync();
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
