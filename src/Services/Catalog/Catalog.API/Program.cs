using Marten;

var builder = WebApplication.CreateBuilder(args);

//Add service to the container

//Register the Carter library in the DI.
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();//Lightweight session for performance.

var app = builder.Build();

//Configure the HTTP request pipeline.

app.MapCarter();

app.Run();
