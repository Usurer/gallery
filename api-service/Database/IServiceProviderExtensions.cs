using Microsoft.Extensions.DependencyInjection;

namespace Database
{
    public static class IServiceProviderExtensions
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
