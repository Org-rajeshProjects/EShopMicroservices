
using BuildingBlocks.Exceptions.Handlers;

var builder = WebApplication.CreateBuilder(args);

//Add service to the container
var assembly = typeof(Program).Assembly;

//Register the Carter library in the DI.
builder.Services.AddCarter();

//MediatR registration
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);//Register all MediatR handlers from the current assembly.
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));//Register the validation behavior in the MediatR pipeline.
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));//Register the logging behavior in the MediatR pipeline.
});

//Marten registration for document database access (using PostgreSQL as the underlying database).
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);//Configure the ShoppingCart document schema with UserName as the identity field.
}).UseLightweightSessions();//Lightweight session for performance.

builder.Services.AddScoped<IBasketRepository, BasketRepository>();//Register the basket repository for data access.

//Register custom exception handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//Configure the HTTP request pipeline.

//Added Carter to the request pipeline to handle routes.
app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
