
using BuildingBlocks.Exceptions.Handlers;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

//Add service to the container
var assembly = typeof(Program).Assembly;

//Application Services
//Register the Carter library in the DI.
builder.Services.AddCarter();

//MediatR registration
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);//Register all MediatR handlers from the current assembly.
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));//Register the validation behavior in the MediatR pipeline.
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));//Register the logging behavior in the MediatR pipeline.
});


//Data Services
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

//Grpc Services
builder.Services.AddGrpcClient<DiscountPrtoService.DiscountPrtoServiceClient>(options =>
{
    // Configure the base address for the generated gRPC client.
    // This tells the client which endpoint to use when creating the underlying channel to call
    // the external Discount gRPC service. The URL is read from configuration ("GrpcSettings:DiscountUrl").
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
   
.ConfigurePrimaryHttpMessageHandler(() =>
{
    // WHY WE USE THIS:
    // The call below customizes the underlying HttpMessageHandler used by the gRPC client.
    // In development or test environments it's common to have self-signed or locally issued TLS certificates.
    // Setting ServerCertificateCustomValidationCallback to DangerousAcceptAnyServerCertificateValidator
    // bypasses certificate validation so the client can connect to such endpoints without SSL errors.
    //
    // WHY THIS MUST NOT BE USED IN PRODUCTION:
    // DangerousAcceptAnyServerCertificateValidator disables all TLS certificate validation.
    // This effectively removes the HTTPS/TLS security guarantees and makes the client vulnerable to
    // man-in-the-middle (MITM) attacks and certificate spoofing. Using it in production exposes
    // sensitive data and trust boundaries and is a serious security risk.
    //
    // PRODUCTION REPLACEMENTS / RECOMMENDATIONS:
    // 1) Use properly issued CA-signed certificates for the gRPC server and ensure the client trusts the CA
    //    via the OS certificate store or a configured trust store. Remove any custom validator.
    // 2) Use environment-specific configuration: enable the dangerous validator only when
    //    builder.Environment.IsDevelopment() or via a specific feature flag.
    // 3) For tighter security, consider certificate pinning or validating specific certificate properties
    //    (thumbprint, subject) rather than accepting any certificate.
    // 4) Use platform-managed secrets for certificates (Azure Key Vault, Kubernetes Secrets, etc.)
    //    and ensure TLS termination is performed correctly in ingress/load balancers or service mesh.
    // 5) If mutual TLS (mTLS) is required, configure client certificates and proper server validation.
    //
    // Example (recommended) production approach: do not set a custom ServerCertificateCustomValidationCallback
    // and rely on default validation, or configure a custom callback that validates against a known thumbprint.
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});


//Cross cutting services
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
