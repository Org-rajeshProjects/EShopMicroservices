namespace Ordering.Domain.ValueObjects;

public record OrderId
{
    public Guid Value { get; }

    private OrderId(Guid value)
    {
        Value = value;
    }

    //The Of method is a factory method that creates an instance of OrderId from a Guid value. It performs validation to ensure that the provided Guid is not null or empty, throwing a DomainException if the validation fails. This approach encapsulates the creation logic and ensures that only valid OrderId instances are created.
    public static OrderId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("OrderId cannot be empty.");
        }

        return new OrderId(value);
    }
}
