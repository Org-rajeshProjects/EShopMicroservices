namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderCreatedEventhandler(ILogger<OrderCreatedEventhandler> logger) : INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Domain event handled: {notification.GetType().Name}");
            return Task.CompletedTask;
        }
    }
}
