namespace Catalog.API.Products.GetProducts;
public record GetProductRequest(int? PageNumber, int? PageSize);
public record GetProductResponse(IEnumerable<Product> Products); // Response DTO containing a list of products returned to the client
public class GetProductsEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters]GetProductRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetProductsQuery>(); // Create a new query to get products
            var result = await sender.Send(query); // Send the query via the mediator to get the result
            var response = result.Adapt<GetProductResponse>(); // Map the result to the response DTO
            return Results.Ok(response); // Return HTTP 200 OK with the response
        })
        .WithName("GetProducts") // Name of the route for documentation and diagnostics
        .Produces<GetProductResponse>(StatusCodes.Status200OK) // Documents that this endpoint produces a 200 OK with GetProductResponse body
        .ProducesProblem(StatusCodes.Status500InternalServerError) // Documents that this endpoint can produce a problem response for server errors (500)
        .WithSummary("Get Products") // Summary for OpenAPI/Swagger UI
        .WithDescription("Retrieve all products"); // Description for OpenAPI/Swagger UI
    }
}
