
namespace Catalog.API.Products.GetProductById
{
    //public record GetProductByIdRequest(Product Product); // Here we dont have any request object because we are passing the id as route parameter. To follow best practice we are keeping it in comment.
    public record GetProductByIdResponse(Product Product); //The parameter in the "GetProductByIdResult(Product product)" Handler class should be exctly same as this response class.

    public class GetProductByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));
                var response = result.Adapt<GetProductByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get product by Id")
            .WithDescription("Get product by Id from the catalog");
        }
    }
}
