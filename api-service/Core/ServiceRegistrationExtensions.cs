using Core.Abstractions;
using Core.Services;
using EasyCaching.Disk;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class ServiceRegistrationExtensions
    {
        public const string MagickResizeServiceKey = "magick";
        public const string FlowResizeServiceKey = "flow";

        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddEasyCaching(options =>
            {
                options.WithMessagePack("disk");
                options.UseInMemory("in-memory");

                options.UseDisk(config =>
                    {
                        config.DBConfig = new DiskDbOptions { BasePath = "C:\\Coding\\Meaningful Projects\\Gallery\\_cache" };
                    },
                    "disk"
                );
            });

            services.AddScoped<ICacheService, CacheService>();
            services.AddKeyedScoped<IImageResizeService, ImagemagicResizeService>(MagickResizeServiceKey);
            services.AddKeyedScoped<IImageResizeService, ImageflowResizeService>(FlowResizeServiceKey);
            services.AddScoped<IImageProviderService, ImageProviderService>();
        }
    }
}