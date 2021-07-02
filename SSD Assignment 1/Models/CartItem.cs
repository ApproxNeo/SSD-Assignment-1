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
        public int CartItemId { get; set; }

        public string UserId { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

    }
}
