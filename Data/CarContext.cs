using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarInfoFromDatabase.Data
{
    public class CarContext:IdentityDbContext<StoreUsers>
    {
        private readonly IConfiguration _config;

        public CarContext(IConfiguration config,DbContextOptions<CarContext> options):base(options)
        {
            this._config = config;
        }

        public DbSet<Root> root { get; set; }
     
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:CarsContextDb"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Root>().HasKey(p=>new { p.carId});
            modelBuilder.Entity<Root>().Property(p => p.carId).ValueGeneratedOnAdd();
        
        }


    }
}
