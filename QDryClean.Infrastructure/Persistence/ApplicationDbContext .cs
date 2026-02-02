using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Domain.Entities;

namespace QDryClean.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> OrderInvoices { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Charge> Charges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("ReceiptNumberSeq")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Order>()
                .Property(o => o.ReceiptNumber)
                .HasDefaultValueSql("NEXT VALUE FOR ReceiptNumberSeq");

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.ReceiptNumber)
                .IsUnique();


            modelBuilder.Entity<Charge>()
                .Property(c => c.Cost)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalCost)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Points)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Discount)
                .HasPrecision(6, 0); // если нужны проценты
        }
    }
}

