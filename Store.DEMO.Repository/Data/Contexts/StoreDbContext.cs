using Microsoft.EntityFrameworkCore;
using Store.DEMO.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Repository.Data.Contexts
{
    public class StoreDbContext : DbContext
    {

        public StoreDbContext(DbContextOptions<StoreDbContext> option):base(option)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }
        public DbSet<ProductType> Types { get; set; }
    }
}
