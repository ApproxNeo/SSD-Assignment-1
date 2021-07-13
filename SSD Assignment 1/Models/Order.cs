using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SSD_Assignment_1.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string OrderDetails { get; set; }
        [Required]
        public string IntentId { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Required]
        public string PaymentStatus { get; set; }



    }
}
