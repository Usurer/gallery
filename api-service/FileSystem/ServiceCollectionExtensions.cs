using Core;
using Microsoft.Extensions.DependencyInjection;

namespace FileSystem
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFileSystemServices(this IServiceCollection services)
        {
            services.AddScoped<IFileSystemService, FileSystemService>();
        }
    }
}
