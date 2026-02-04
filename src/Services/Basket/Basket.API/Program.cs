
using BuildingBlocks.Exceptions.Handlers;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;

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
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();//Decorate the basket repository with caching functionality using Scrutor.Decorate.

//Manually register the CachedBasketRepository with Decorator pattern to add caching functionality.
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetRequiredService<BasketRepository>();
//    return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
//});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket_";
});//Register Redis distributed cache for caching basket data.

//Register custom exception handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

//Adding health checks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)//Here we add a health check for PostgreSQL from "AspNetCore.HealthCheks.NpgSql" using the connection string from configuration. !means we are sure it is not null.
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);//Add Redis health check from "AspNetCore.HealthCheks.Redis"  package.

var app = builder.Build();

//Configure the HTTP request pipeline.

//Added Carter to the request pipeline to handle routes.
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,//Check all registered health checks.
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});//Map health check endpoint at /health with a custom response writer.
app.Run();
