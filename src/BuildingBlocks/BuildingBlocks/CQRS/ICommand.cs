using MediatR;

namespace BuildingBlocks.CQRS;

//Command handler without a reponse.
public interface ICommand : ICommand<Unit>
{

}

//Command handler that should have a response.
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
