namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public record GetOrdersbyNameQuery(string Name):IQuery<GetOrdersByNameResult>;

public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);

