using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSD_Assignment_1.Models
{
    public class CartItem
    {
        [Key]
        public string CartID;
        public string UserID;
        public string ProductID;
        public int Quantity;

    }
}
