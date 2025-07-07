using Microsoft.EntityFrameworkCore;
using VerstaTest.Application.Interfaces;
using VerstaTest.Domain;

namespace VerstaTest.WebApi;

public class ApplicationContext : DbContext, IApplicationDbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(Environment.CurrentDirectory, "MyDB.db");

        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne(i => i.Customer)
            .WithMany(i => i.Orders)
            .HasForeignKey(i => i.CustomerId);
    }
}
