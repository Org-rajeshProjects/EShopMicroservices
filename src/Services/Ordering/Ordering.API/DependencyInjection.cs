namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Register API services here
            // e.g., controllers, Swagger, etc.
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            // Configure API middleware here
            // e.g., app.UseAuthentication(), app.UseAuthorization(), etc.
            return app;
        }
    }
}
