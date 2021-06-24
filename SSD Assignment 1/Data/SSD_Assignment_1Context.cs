using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SSD_Assignment_1.Models;

namespace SSD_Assignment_1.Data
{
    public class SSD_Assignment_1Context : DbContext
    {
        public SSD_Assignment_1Context (DbContextOptions<SSD_Assignment_1Context> options)
            : base(options)
        {
        }

        public DbSet<SSD_Assignment_1.Models.Product> Product { get; set; }
    }
}
