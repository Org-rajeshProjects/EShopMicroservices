using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public static class Extensions
    {
        //This is to auto migrate the DB on application startup.
        public static IApplicationBuilder UseMigration(this IApplicationBuilder application)
        {
            using var scope = application.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetService<DiscountContex>();
            context?.Database.MigrateAsync();

            return application;
        }
    }
}
