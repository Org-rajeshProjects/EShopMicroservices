
namespace Catalog.API.Products.DeleteProduct;

//public record DeleteProductRequest(Guid Id); // Request DTO containing the ID of the product to be deleted

public record DeleteProductResponse(bool IsSuccess); // Response DTO indicating whether the deletion was successful
public class DeleteProductEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id)); // Send the command to the mediator

            var response = result.Adapt<DeleteProductResponse>(); // Map the result to the response DTO

            return Results.Ok(response); // Return the response

        }).WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Deletes a product by its ID")
        .WithDescription("Deletes a product from the catalog using the provided product ID.");
    }
}
