
namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery() : IQuery<GetProductResult>; // Query to retrieve all products, expects a list of Product entities in response
public record GetProductResult(IEnumerable<Product> Products); // Result containing the list of products retrieved by the query

public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);

        var products = await session.Query<Product>().ToListAsync(cancellationToken);

        return new GetProductResult(products);
    }
}
