using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handlers;

var builder = WebApplication.CreateBuilder(args);

//Add service to the container

//Register the Carter library in the DI.
builder.Services.AddCarter();

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
//FluentValidation registration
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();//Lightweight session for performance.

//Register custom exception handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//Configure the HTTP request pipeline.

app.MapCarter();

//Use the exception handler middleware
//empty options to enable the middleware, as the actual handling is done in BuildingBlocks.Exceptions.Handlers.CustomExceptionHandler
app.UseExceptionHandler(options => { });

//Commented out custom exception handler in favor of BuildingBlocks.Exceptions.Handlers.CustomExceptionHandler to use IExceptionHandler implementations.
//app.UseExceptionHandler(app =>
//{
//    app.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if (exception == null)
//            return;

//        var problemDetails = new ProblemDetails
//        {
//            Title = exception.Message,
//            Status = StatusCodes.Status500InternalServerError,
//            Detail = exception.StackTrace
//        };

//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, exception.Message);

//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/problem+json";

//        await context.Response.WriteAsJsonAsync(problemDetails);

//    });
//});
app.Run();
