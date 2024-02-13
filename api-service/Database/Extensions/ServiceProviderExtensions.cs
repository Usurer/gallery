using Microsoft.Extensions.DependencyInjection;

namespace Database.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void UseSqliteDb(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GalleryContext>();
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
