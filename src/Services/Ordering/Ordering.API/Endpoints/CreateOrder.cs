using Ordering.Application.Orders.Command.CreateOrder;

namespace Ordering.API.Endpoints;

//Accept a CreateOrderRequest object.
//Maps the request to a CreateOrderCommand
//Use MediatR to send the command to the corresponding handler.
//Returns a response with the created order's ID.

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateOrderCommad>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreateOrderResponse>();

            return Results.Created($"/orders/{response.Id}", response);
        })
        .WithName("Create Order")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new order")
        .WithDescription("Creates a new order based on the provided order details.");
    }
}
