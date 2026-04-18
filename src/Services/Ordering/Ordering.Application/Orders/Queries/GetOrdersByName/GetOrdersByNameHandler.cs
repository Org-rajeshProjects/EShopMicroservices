
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public class GetOrdersByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersbyNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersbyNameQuery request, CancellationToken cancellationToken)
    {

        var orders = await dbContext.Orders.Include(o => o.OrderItems).AsNoTracking().Where(o => o.OrderName.Value.Contains(request.Name)).OrderBy(o => o.OrderName).ToListAsync(cancellationToken);


        return new GetOrdersByNameResult(orders.ToOrderDtoList());
    }

}


