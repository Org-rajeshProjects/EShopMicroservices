using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContex:DbContext
{
    public DiscountContex(DbContextOptions<DiscountContex> options):base(options)
    {

    }

    public DbSet<Coupon> Coupons { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon { Id = 1, ProductName = "IPhone X", Description = "IPhone Discount", Amount = 150 },
            new Coupon { Id = 2, ProductName = "Samsung 10", Description = "Samsung Discount", Amount = 100 }
            );

        base.OnModelCreating(modelBuilder);//This is used to call the base class's implementation of the OnModelCreating method, which is important for ensuring that any additional configuration defined in the base class is also applied to the model.
    }
}
