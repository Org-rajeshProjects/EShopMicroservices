namespace Catalog.API.Products.CreateProduct; // Groups related types for the Create Product feature under a clear namespace

// DTO that represents the shape of the incoming HTTP request body for creating a product.
// - Simple carrier type used by model binding to deserialize JSON into these properties.
// - Keeping request types separate from domain/command types helps decouple transport concerns from application logic.
public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

// DTO returned to the client after a product is created.
// - Wrapping the response in a dedicated type (instead of returning Guid directly) makes it easy to extend later (add links, metadata, etc.).
public record CreateProductResponse(Guid Id);

// Carter module that registers HTTP routes for this feature.
// - Implementing ICarterModule allows the Carter framework (built on top of ASP.NET Core minimal APIs) to discover and add routes.
public class CreateProductEndPoint : ICarterModule
{
    // Called by the framework to register routes/endpoints.
    // - IEndpointRouteBuilder is the ASP.NET Core abstraction used to map routes.
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Map an HTTP POST to /products.
        // - The route handler uses minimal-API style parameters: CreateProductRequest is model-bound from the request body,
        //   ISender is resolved from DI (a mediator/sender that dispatches commands to their handlers).
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            // Convert the incoming request DTO to an application command.
            // - `Adapt<T>` is a mapping convenience (e.g., from Mapster). It creates a CreateProductCommand
            //   populated from the matching properties on CreateProductRequest.
            var command = request.Adapt<CreateProductCommand>();

            // Send the command to the mediator (ISender).
            // - The mediator locates the appropriate ICommandHandler<CreateProductCommand, CreateProductResult>
            //   and invokes it. This keeps the endpoint thin and delegates business logic to the handler.
            // - Awaiting the call allows the handler to perform async work (DB access, etc.) without blocking threads.
            var result = await sender.Send(command);

            // Map the handler result to the HTTP response DTO.
            // - This separates the internal result shape from the API contract returned to clients.
            var response = result.Adapt<CreateProductResponse>();

            // Return HTTP 201 Created, include a Location-like path and the response body.
            // - Results.Created sets the 201 status and serializes the response object as JSON.
            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct") // Assigns a route name for link generation, documentation, and diagnostics.
        .Produces<CreateProductResponse>(StatusCodes.Status201Created) // Documents that this endpoint can produce a 201 with CreateProductResponse body.
        .ProducesProblem(StatusCodes.Status400BadRequest) // Documents that this endpoint can produce a problem response for bad requests (400).
        .WithSummary("Create Product") // Small summary used by OpenAPI/Swagger UI.
        .WithDescription("Create Product"); // Longer description used by OpenAPI/Swagger UI.
    }
}
