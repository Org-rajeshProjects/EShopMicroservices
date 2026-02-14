namespace Basket.API.Models;

public class ShoppingCartItem
{
    public int Quantity { get; set; } = default!;//We are using default! to suppress nullable warnings for properties that will be set later. default! tells the compiler that we are aware the property is not initialized here, but it will be set before use.
    public string Color { get; set; } = default!;
    public double Price { get; set; } = default!;
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; } = default!;
}
