
namespace Catalog.API.Products.GetProductByCatagory;

public record GetProductByCategoryQuery(string Catagory) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);
internal class GetProductByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {

        var products = await session.Query<Product>()
            .Where(x => x.Catagory.Contains(query.Catagory))
            .ToListAsync();

        return new GetProductByCategoryResult(products);
    }
}
