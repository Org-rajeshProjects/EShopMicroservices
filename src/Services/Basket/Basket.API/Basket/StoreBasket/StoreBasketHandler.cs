namespace Basket.API.Basket.StoreBasket;

public record StorebasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StorebasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(command => command.Cart).NotNull().WithMessage("Cart cannot be null.");
        RuleFor(command => command.Cart.UserName).NotEmpty().WithMessage("UserName is required.");  
    }
}
public class StoreBasketCommandHandler(IBasketRepository repository) : ICommandHandler<StorebasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StorebasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart cart= command.Cart;

       await repository.StoreBasketAsync(cart, cancellationToken);
    return new StoreBasketResult(command.Cart.UserName);

    }
}
