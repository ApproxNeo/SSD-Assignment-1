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
        [Key]
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(20)]
        public string Name { get; set; }

        [Required, MinLength(3), MaxLength(20)]
        public string Brand { get; set; }

        [Required, MinLength(3), MaxLength(20)]
        public string Genre { get; set; }

        [Required, MinLength(3), MaxLength(20)]
        public string Animal { get; set; }

        [Required, MaxLength(300)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        //[Required]
        public string PhotoPath { get; set; }
    }
}
