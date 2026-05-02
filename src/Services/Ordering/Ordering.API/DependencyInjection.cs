using BuildingBlocks.Exceptions.Handlers;
using Carter;
using HealthChecks.UI.Client;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register API services here
            // e.g., controllers, Swagger, etc.
            services.AddCarter();

            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddHealthChecks().AddSqlServer(configuration.GetConnectionString("Database")!);
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            // Configure API middleware here
            // e.g., app.UseAuthentication(), app.UseAuthorization(), etc.
            app.MapCarter();
            app.UseExceptionHandler(options => { });
            app.UseHealthChecks("/health",
                new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            return app;
        }
    }
}
