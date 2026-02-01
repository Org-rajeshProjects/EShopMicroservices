namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>; // Command to delete a basket for a given user, returns a boolean indicating success
public record DeleteBasketResult(bool IsSuccess); // Result indicating whether the basket deletion was successful

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(command => command.UserName).NotEmpty().WithMessage("UserName is required.");
    }
}
public class DeleteBasketCommandHandler(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteBasketAsync(command.UserName, cancellationToken);
        return new DeleteBasketResult(true);
    }
}
