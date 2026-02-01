
namespace Basket.API.Basket.DeleteBasket;

//public record DeleteBasketRequest(string UserName);

public record DeleteBasketResponse(bool IsSuccess); // Response DTO indicating whether the deletion was successful
public class DeleteBasketEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(userName));

            var response = result.Adapt<DeleteBasketResponse>();

            return Results.Ok(response);
        })
        .WithName("Deletebasket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Basket by user name")
        .WithDescription("Delete Basket by user name");
    }
}
