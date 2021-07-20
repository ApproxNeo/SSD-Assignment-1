using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SSD_Assignment_1.Pages.Shared
{
    public class Layout
    {

        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;

        public Layout(SSD_Assignment_1.Data.SSD_Assignment_1Context context)
        {
            _context = context;
        }

        [BindProperty]
        public string SearchString { get; set; }
    }
}
