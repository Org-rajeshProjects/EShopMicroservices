
using System.Net.WebSockets;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price); // Request DTO containing product details to be updated

public record UpdateProductResponse(bool IsSuccess); // Response DTO indicating whether the update was successful
public class UpdateProductEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {

            var command = request.Adapt<UpdateProductCommand>();// Map the request to UpdateProductCommand

            var result = await sender.Send(command);// Send the command to the mediator

            var response = result.Adapt<UpdateProductResponse>();// Map the result to UpdateProductResponse
        })
        .WithName("UpdateProduct") // Name the endpoint
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK) // Specify the response type and status code
        .ProducesProblem(StatusCodes.Status400BadRequest) // Specify possible error response
        .ProducesProblem(StatusCodes.Status404NotFound) // Specify possible error response
        .WithSummary("Updates an existing product in the catalog") // Add summary
        .WithDescription("Update an existing product"); // Add description
    }
}
