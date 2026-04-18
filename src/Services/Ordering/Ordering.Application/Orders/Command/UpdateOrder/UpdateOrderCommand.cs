namespace Ordering.Application.Orders.Command.UpdateOrder;

public record UpdateOrderCommand(OrderDto OrderDto) : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool status);

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.OrderDto.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.OrderDto.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.OrderDto.CustomerId).NotEmpty().WithMessage("customerId is required");
    }
}