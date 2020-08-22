using GroceryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CustomerTransaction>().Property(p => p.SoldAmount).HasDefaultValue(0.00);
            //modelBuilder.Entity<CustomerTransaction>().Property(p => p.ReceivedAmount).HasDefaultValue(0.00);

            modelBuilder.Entity<CustomerTransaction>()
            .HasOne(c => c.Customer)
            .WithMany(t => t.CustomerTransactions)
            .HasForeignKey(t=>t.CustomerId)            
            .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);           
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerTransaction> CustomerTransactions { get; set; }

    }
}
