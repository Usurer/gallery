namespace Core.Abstractions
{
    internal interface ICacheService
    {
        Task<bool> SetAsync<T>(string key, T value);

        bool TryGet<T>(string key, out T value);
    }
}