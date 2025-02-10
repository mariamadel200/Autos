using Autos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Autos.Data
{
    public class AutoDbContext:IdentityDbContext<User>
    {
        public AutoDbContext(DbContextOptions<AutoDbContext> options)
    : base(options)
        {

        }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<VehicleForRent> VehiclesForRent { get; set; }
        public DbSet<VehicleForSale> VehiclesForSale { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        internal static object GetService(Type type)
        {
            throw new NotImplementedException();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()

                },
                 new IdentityRole()
                 {
                     Id = Guid.NewGuid().ToString(),
                     Name = "User",
                     NormalizedName = "USER",
                     ConcurrencyStamp = Guid.NewGuid().ToString()

                 },

                 new IdentityRole()
                 {
                     Id = Guid.NewGuid().ToString(),
                     Name = "SuperAdmin",
                     NormalizedName = "SUPERADMIN",
                     ConcurrencyStamp = Guid.NewGuid().ToString()

                 }


                ) ;
            base.OnModelCreating(builder);
        }
    }
}
