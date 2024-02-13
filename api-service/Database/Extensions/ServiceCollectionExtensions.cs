using Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ConnectionStringName = "sqlite";

        public static void AddSqliteDbStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GalleryContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString(ConnectionStringName));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IStorageService, DatabaseStorageService>();
            services.AddScoped<IScanStorageService, ScanStorageService>();
        }
    }
}
