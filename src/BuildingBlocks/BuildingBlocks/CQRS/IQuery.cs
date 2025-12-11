using MediatR;

namespace BuildingBlocks.CQRS;

public interface IQuery<out TReasponse> : IRequest<TReasponse> where TReasponse : notnull
{
}
