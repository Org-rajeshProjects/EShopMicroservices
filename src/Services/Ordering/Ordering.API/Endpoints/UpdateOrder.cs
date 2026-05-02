
using Ordering.Application.Orders.Command.UpdateOrder;

namespace Ordering.API.Endpoints;


public record UpdateOrderRequest(OrderDto Order);

public record UpdateOrderResponse(bool IsSuccess);
public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
        {
            var command= request.Adapt<UpdateOrderCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<UpdateOrderResponse>();

            return Results.Ok(response);

        })
        .WithName("UpdateOrder")
        .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update an existing order")
        .WithDescription("Update an existing order with the provided details.");

    }
}
