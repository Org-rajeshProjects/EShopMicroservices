using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Extensions;

public class InitialData
{
    public static IEnumerable<Customer> Customers =>
    new List<Customer>
    {
        Customer.Create(CustomerId.Of(new Guid("3b7e4521-8f92-4d5a-b6c1-90ef32415d8a")),"Michale","michale@exe.com"),
        Customer.Create(CustomerId.Of(new Guid("f0a2d984-6c3e-4b71-a582-1d7f62e390bc")),"Jhon","jhon@exe.com"),
    };

    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("ac146d19-d004-4ae2-bcbd-fd1230971918")),"Product 1", 199),
            Product.Create(ProductId.Of(new Guid("22cf8da2-3433-43d4-be02-10509aa56eb4")),"Product 2", 299),
            Product.Create(ProductId.Of(new Guid("5dc57176-f77a-434a-a2b8-393b59952ea7")),"Product 3", 399),
            Product.Create(ProductId.Of(new Guid("9f8ad72b-adcc-454f-83a1-d7a7c0a2a89a")),"Product 4", 499),
        };

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of("Ragnar", "Lothbrok", "ragnar@odin.com", "Valhalla 001/001", "Northumria", "Wessex", "12345");
            var address2 = Address.Of("Loki", "Lothbrok", "loki@odin.com", "Valhalla 002/002", "Northumria", "Wessex", "12345");

            var payment1 = Payment.Of("Ragnar", "1111222233334444", "12/30", "007", 1);
            var payment2 = Payment.Of("Ragnar", "1111222233334444", "12/30", "018", 1);

            var order1 = Order.Create(
                OrderId.Of(new Guid("26087284-664b-4dcd-b009-edc24f6b57fa")),
                CustomerId.Of(new Guid("3b7e4521-8f92-4d5a-b6c1-90ef32415d8a")),
                OrderName.Of("ORD_1"),
                shippingAddress: address1,
                billingAddress: address1,
                payment1);

            order1.Add(ProductId.Of(new Guid("ac146d19-d004-4ae2-bcbd-fd1230971918")), 2, 398);
            order1.Add(ProductId.Of(new Guid("22cf8da2-3433-43d4-be02-10509aa56eb4")), 1, 299);

            var order2 = Order.Create(
               OrderId.Of(new Guid("80b2b85a-d510-4119-a69c-be06ee8cc7d4")),
               CustomerId.Of(new Guid("f0a2d984-6c3e-4b71-a582-1d7f62e390bc")),
               OrderName.Of("ORD_2"),
               shippingAddress: address2,
               billingAddress: address2,
               payment2);

            order2.Add(ProductId.Of(new Guid("5dc57176-f77a-434a-a2b8-393b59952ea7")), 2, 798);
            order2.Add(ProductId.Of(new Guid("9f8ad72b-adcc-454f-83a1-d7a7c0a2a89a")), 1, 499);

            return new List<Order> { order1, order2 };
        }

    }

}
