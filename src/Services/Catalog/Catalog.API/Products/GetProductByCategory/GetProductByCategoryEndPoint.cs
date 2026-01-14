
namespace Catalog.API.Products.GetProductByCatagory;

//public record GetProductByCatagoryRequest(string Catagory); // Request DTO containing the category to filter products
public record GetProductByCategoryResponse(IEnumerable<Product> Products); // Response DTO containing a list of products returned to the client
public class GetProductByCategoryEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/product/category/{category}", async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByCategoryQuery(category));
            var response = result.Adapt<GetProductByCategoryResponse>();
            return Results.Ok(response);
        }).WithName("GetProductsByCategory")
          .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithSummary("Get Product by Category")
          .WithDescription("Get a list of products filtered by the specified category.");
    }
}
