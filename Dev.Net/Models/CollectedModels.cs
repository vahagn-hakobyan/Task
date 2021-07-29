using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dev.Net.Models
{
    public class CollectedModels : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Car> cars { get; set; }
        public CollectedModels(DbContextOptions x) : base(x) { }

    }
}
