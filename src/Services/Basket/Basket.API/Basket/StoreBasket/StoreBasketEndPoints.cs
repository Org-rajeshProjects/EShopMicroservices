
namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string UserName);

public class StoreBasketEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<StorebasketCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<StoreBasketResponse>();

            return Results.Created($"/basket/{response.UserName}", response);
        })
        .WithName("CreateBasket")
        .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Creates a new shopping basket for a user.")
        .WithDescription("Creates a new shopping basket for a user with the provided shopping cart details.");
    }
}
