using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).HasConversion(orderId => orderId.Value, dbId => OrderId.Of(dbId));

        builder.HasOne<Customer>().WithMany().HasForeignKey(o => o.CustomerId).IsRequired();

        //Here we are configuring the relationship between Order and OrderItem. An Order can have many OrderItems, but each OrderItem is associated with one Order. The foreign key in the OrderItem entity is OrderId, which references the Id of the Order.
        builder.HasMany(o => o.OrderItems).WithOne().HasForeignKey(oi => oi.OrderId);

        builder.ComplexProperty(
            o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(n=>n.Value).HasColumnName(nameof(Order.OrderName))
                .HasMaxLength(100).IsRequired();
            });

        builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
        {
            addressBuilder.Property(a=>a.FirstName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.EmailAddress).HasMaxLength(50);
            addressBuilder.Property(a => a.AddressLine).HasMaxLength(180);
            addressBuilder.Property(a => a.Country).HasMaxLength(50);
            addressBuilder.Property(a=>a.State).HasMaxLength(50);
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(5);

        });

        builder.ComplexProperty(o => o.BillingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.EmailAddress).HasMaxLength(50);
            addressBuilder.Property(a => a.AddressLine).HasMaxLength(180);
            addressBuilder.Property(a => a.Country).HasMaxLength(50);
            addressBuilder.Property(a => a.State).HasMaxLength(50);
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(5);

        });

        builder.ComplexProperty(o => o.Payment, paymentBuilder =>
        {
            paymentBuilder.Property(p => p.CardName).HasMaxLength(50);
            paymentBuilder.Property(p=>p.CardNumber).HasMaxLength(24).IsRequired();
            paymentBuilder.Property(p => p.Expiration).HasMaxLength(10);
            paymentBuilder.Property(p => p.CVV).HasMaxLength(3);
            paymentBuilder.Property(p => p.PaymentMethod); 
        });
    }
}
