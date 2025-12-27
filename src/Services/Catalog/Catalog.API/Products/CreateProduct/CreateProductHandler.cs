namespace Catalog.API.Products.CreateProduct;

// Command (data carrier) used to request product creation. It implements ICommand<CreateProductResult>.
// - The generic parameter CreateProductResult tells the system: "When this command is handled, expect a CreateProductResult back".
// - Using ICommand<TResponse> gives compile-time guarantees about what the handler must return and helps mediator/CQRS libraries route and type-check messages.
// - Commands typically represent intent to change state (create/update/delete) and are distinct from queries (which return data without side effects).
public record CreateProductCommand(string Name, List<string> Catagory, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>; // DTO/command that carries data required to create a product and declares the expected response type

// The response (result) produced after handling a CreateProductCommand.
// - Keeping the result as a separate type (instead of returning e.g. Guid directly) makes it easy to extend the response later (add warnings, links, metadata).
// - The handler will construct and return this type to the caller (often a controller or mediator caller).
public record CreateProductResult(Guid Id); // Result returned after creating a product, contains the new product's Id

// Command handler class responsible for executing the CreateProductCommand.
// - Primary constructor injects the persistence/session dependency (IDocumentSession).
// - Implementing ICommandHandler<CreateProductCommand, CreateProductResult> makes this class discoverable by mediator/CQRS infrastructure
//   and enforces the contract: it accepts a CreateProductCommand and returns a CreateProductResult asynchronously.
internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult> // Command handler with an injected persistence session
{
    // Handle method invoked by the mediator/CQRS dispatcher to process the command.
    // - Returns a Task<CreateProductResult> because handling is asynchronous (I/O such as DB calls).
    // - CancellationToken allows the caller to request cancellation (e.g., when an HTTP request is aborted).
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken) // Entry point invoked to handle the CreateProductCommand asynchronously
    {
        // Business logic to create a product. Typical places for rules:
        // - Validate inputs
        // - Enforce invariants
        // - Apply domain rules
        // (Validation is omitted here for brevity.)

        var product = new Product 
        {
            Name = command.Name, 
            Catagory = command.Catagory, 
            Description = command.Description, 
            ImageFile = command.ImageFile, 
            Price = command.Price, 
        };

        // Persist the new product using the injected document/session.
        // - session.Store registers the entity with the unit-of-work/session.
        // - SaveChangesAsync commits the changes to the underlying database asynchronously.
        session.Store(product); // Register the new product instance with the session so it will be saved
        await session.SaveChangesAsync(cancellationToken); // Commit changes to the database asynchronously, honoring cancellation token

        // Return the CreateProductResult containing the newly created product Id.
        // - The caller (e.g., controller) receives this strongly-typed result and can act on it (return HTTP 201 with location, etc.).
        return new CreateProductResult(product.Id); // Return the result containing the newly created product Id
    }
}
