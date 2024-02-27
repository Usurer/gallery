namespace Core.Abstractions
{
    public interface IImageProviderService
    {
        Task<ImageResizeResult?> GetResizedAsync(long id, int updatedAtDate, int? width, int? height);

        Task<ImageResizeResult?> GetOriginalAsync(long id);
    }
}