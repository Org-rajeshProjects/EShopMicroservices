namespace BuildingBlocks.Messaging.Events;

// This event is published when a user checks out their shopping basket. It contains information about the checkout process, such as the user's ID, the items in the basket, and the total amount. Other services can subscribe to this event to perform actions such as processing payments, updating inventory, or sending confirmation emails.
//inherits from IntegrationEvent, which means it will have the properties defined in the IntegrationEvent class, such as Id, OccurredOn, and EventType. This allows it to be used in a messaging system that relies on these properties for event handling and processing.
public record BasketCheckoutEvent : IntegrationEvent
{
    public string UserName { get; set; } = default!;
    public Guid CustomerId { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;

    //Shipping And Billing Address
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string AddresLine { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;

    //Payment
    public string CardName { get; set; } = default!;
    public string CardNumber { get; set; } = default!;
    public string Expiration {  get; set; } = default!;
    public string CVV { get; set; } = default!;
    public int PaymentMethod { get; set; } = default!;
}
