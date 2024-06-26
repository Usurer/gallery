using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void CheckDbState(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GalleryContext>();
                if (!dbContext.Database.CanConnect())
                {
                    throw new ApplicationException($"Cannot connect to DB {dbContext.Database.GetConnectionString()}");
                }

                var migrations = dbContext.Database.GetPendingMigrations();
                if (migrations.Any())
                {
                    throw new ApplicationException($"Database is not up to date, {string.Join(';', migrations)}");
                }
            }
        }
    }
}