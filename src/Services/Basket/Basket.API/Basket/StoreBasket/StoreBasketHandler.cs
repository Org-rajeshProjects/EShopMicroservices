using Discount.Grpc.Protos;
using JasperFx.Events.Daemon;

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
public class StoreBasketCommandHandler(IBasketRepository repository, DiscountPrtoService.DiscountPrtoServiceClient discountPrtoServiceClient) : ICommandHandler<StorebasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StorebasketCommand command, CancellationToken cancellationToken)
    {
        //To Communicate with the Discount.Grpc and calculate the latest prices of the products.
        await DeductDiscount(command.Cart, cancellationToken);

        await repository.StoreBasketAsync(command.Cart, cancellationToken);
        return new StoreBasketResult(command.Cart.UserName);

    }

    public async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await discountPrtoServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);

            item.Price -= coupon.Amount;
        }

    }
}
