var builder = WebApplication.CreateBuilder(args);

//Add service to the container

//Register the Carter library in the DI.
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

var app = builder.Build();

//Configure the HTTP request pipeline.

app.MapCarter();

app.Run();
