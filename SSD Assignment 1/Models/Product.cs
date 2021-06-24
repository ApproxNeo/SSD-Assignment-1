using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SSD_Assignment_1.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Genre { get; set; }
        public string Animal { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
