using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(customerId => customerId.Value, dbId => CustomerId.Of(dbId));
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(255);

        //HasIndex is used to create an index on the Email property, which can improve query performance when searching for customers by email. The IsUnique method ensures that the email address is unique across all customers, preventing duplicate entries in the database.
        builder.HasIndex(c => c.Email).IsUnique();
    }
}
