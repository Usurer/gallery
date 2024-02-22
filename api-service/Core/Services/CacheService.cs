using Core.Abstractions;
using EasyCaching.Core;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    internal class CacheService : ICacheService
    {
        private readonly IEasyCachingProvider EasyCachingProvider;
        private readonly ILogger<CacheService> Logger;

        public CacheService(IEasyCachingProviderFactory easyCachingProviderFactory, ILogger<CacheService> logger)
        {
            EasyCachingProvider = easyCachingProviderFactory.GetCachingProvider("disk");
            Logger = logger;
        }

        public bool TryGet<T>(string key, out T value)
        {
            var cacheResult = EasyCachingProvider.Get<T>(key);
            if (cacheResult?.HasValue == true)
            {
                value = cacheResult.Value;
                return true;
            }

            value = default;
            return false;
        }

        public async Task<bool> SetAsync<T>(string key, T value)
        {
            try
            {
                EasyCachingProvider.Set<T>(key, value, TimeSpan.FromDays(1));
                return true;
            }
            catch (DirectoryNotFoundException ex)
            {
                Logger.LogError(ex, "Cache failure, directory not found");
                // If the cache folder was removed while the app is running, there seems to be
                // no way to recreate it and FlushAsync I'm using here doesn't help.
                // TODO: Find a way to re-instantiate stuff that was setup on app start
                await EasyCachingProvider.FlushAsync();
                return false;
            }
        }
    }
}