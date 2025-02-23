using EmailApp.Models.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EmailApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Subscribed> Subscriptions { get; set; }

        //        protected override void OnModelCreating(ModelBuilder modelBuilder)
        //        {
        //            base.OnModelCreating(modelBuilder);
        //        }

        //public ApplicationDbContext() { }


        ///*        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //                // Provide a fallback connection string for design-time operations
        //                optionsBuilder.UseSqlServer("Server=EPINHYDW1327\\SQLEXPRESS;Database=EmailAppDB;Trusted_Connection=True;TrustServerCertificate=true");
        //            }
        //        }*/

    }
}
