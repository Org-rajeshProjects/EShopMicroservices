using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id).HasConversion(orderItemId => orderItemId.Value, dbId => OrderItemId.Of(dbId));

        //Here the relation between OrderItem and Product is configured as a one-to-many relationship, where one Product can be associated with many OrderItems. The foreign key is specified as ProductId in the OrderItem entity.
        builder.HasOne<Product>().WithMany().HasForeignKey(oi => oi.ProductId);


        builder.Property(oi => oi.Quantity).IsRequired();
        builder.Property(oi => oi.Price).IsRequired();
    }
}
