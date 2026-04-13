using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // Since this method is synchronous, we need to block on the asynchronous DispatchDomainEvents method. This is generally not recommended due to potential deadlocks, but in this case, we will assume that the domain events are not doing any I/O-bound work that could cause a deadlock. If your domain events involve I/O operations, consider using the asynchronous version of this interceptor instead.
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null) return;

        // Retrieve all entities that implement IAggregate and have domain events.
        var aggregates = context.ChangeTracker.Entries<IAggregate>().Where(a => a.Entity.DomainEvents.Any()).Select(a => a.Entity);

        // Collect all domain events from the aggregates.
        var domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList();

        aggregates.ToList().ForEach(a => a.ClearDomainEvents());// Clear the domain events after collecting them to avoid potential issues with reentrancy or multiple dispatches.

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}
