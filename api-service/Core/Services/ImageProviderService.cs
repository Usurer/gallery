using Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    internal class ImageProviderService : IImageProviderService
    {
        private readonly IImageResizeService FlowResizeService;
        private readonly IImageResizeService MagickResizeService;
        private readonly ICacheService CacheService;
        private readonly IFileSystemService FileSystemService;
        private readonly ILogger<ImageProviderService> Logger;

        public ImageProviderService(
            [FromKeyedServices(ServiceRegistrationExtensions.FlowResizeServiceKey)] IImageResizeService flowResizeService,
            [FromKeyedServices(ServiceRegistrationExtensions.MagickResizeServiceKey)] IImageResizeService magickResizeService,
            ICacheService cacheService,
            IFileSystemService fileSystemService,
            ILogger<ImageProviderService> logger
        )
        {
            FlowResizeService = flowResizeService;
            MagickResizeService = magickResizeService;
            CacheService = cacheService;
            FileSystemService = fileSystemService;
            Logger = logger;
        }

        public async Task<ImageResizeResult?> GetResizedAsync(long id, int updatedAtDate, int? width, int? height)
        {
            var cacheKey = $"{id}_{updatedAtDate}_{width}_{height}";

            var cacheFound = CacheService.TryGet(cacheKey, out ImageResizeResult cacheValue);
            if (!cacheFound)
            {
                using var imageData = FileSystemService.GetImage(id);

                if (imageData == null)
                {
                    return null;
                }

                var useMagick = string.Compare(imageData.Info.Extension, ".cr2", StringComparison.OrdinalIgnoreCase) == 0
                    || string.Compare(imageData.Info.Extension, ".arw", StringComparison.OrdinalIgnoreCase) == 0;

                var result = useMagick
                    ? await MagickResizeService.GetAsync(imageData, width, height)
                    : await FlowResizeService.GetAsync(imageData, width, height);

                try
                {
                    await CacheService.SetAsync(cacheKey, result);
                }
                catch (DirectoryNotFoundException ex)
                {
                    Logger.LogError(ex, "Cache failure, directory not found");
                    // If the cache folder was removed while the app is running, there seems to be no way to recreate it
                    // TODO: Find a way to re-instantiate stuff that was setup on app start
                }

                return result;
            }

            return cacheValue;
        }

        public async Task<ImageResizeResult?> GetOriginalAsync(long id)
        {
            using var imageData = FileSystemService.GetImage(id);

            if (imageData == null)
            {
                return null;
            }

            var useMagick = string.Compare(imageData.Info.Extension, ".cr2", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(imageData.Info.Extension, ".arw", StringComparison.OrdinalIgnoreCase) == 0;

            return useMagick
                ? await MagickResizeService.GetAsync(imageData, null, null)
                : await FlowResizeService.GetAsync(imageData, null, null);
        }
    }
}