using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Command.CreateOrder;

public record CreateOrderCommad(OrderDto Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid id);

public class CreateOrderCommandValidator: AbstractValidator<CreateOrderCommad>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("customerId is required");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty");
    }
}
