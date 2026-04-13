using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using System.Reflection;

namespace Ordering.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected ApplicationDbContext()
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();//Set() is a method from DbContext that returns a DbSet<TEntity> for the specified entity type. It allows us to perform CRUD operations on the Customer entities in the database.

    public DbSet<Product> Products => Set<Product>(); 
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Customer>().Property(c=>c.Name).IsRequired().HasMaxLength(100);

        //Apply all configurations from the assembly where the current executing code is located. This allows us to automatically apply any entity configurations defined in separate classes that implement IEntityTypeConfiguration<T>.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

}
