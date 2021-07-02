using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SSD_Assignment_1.Models;

namespace SSD_Assignment_1.Pages.Cart
{
    public class CartManager
    {

        private readonly SSD_Assignment_1.Data.SSD_Assignment_1Context _context;
        public CartManager(SSD_Assignment_1.Data.SSD_Assignment_1Context context) {
             _context = context;
        }

         

      }
}
