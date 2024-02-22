using Core.Abstractions;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    internal class ImageProviderService : IImageProviderService
    {
        private readonly IImageResizeService ImageResizeService;
        private readonly ICacheService CacheService;
        private readonly IFileSystemService FileSystemService;
        private readonly ILogger<ImageProviderService> Logger;

        public ImageProviderService(IImageResizeService imageResizeService, ICacheService cacheService, IFileSystemService fileSystemService, ILogger<ImageProviderService> logger)
        {
            ImageResizeService = imageResizeService;
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

                var result = await ImageResizeService.GetAsync(imageData, width, height);

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

        public FileItemData? GetOriginal(long id)
        {
            return FileSystemService.GetImage(id);
        }
    }
}