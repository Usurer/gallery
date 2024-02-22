namespace Core.Abstractions
{
    public interface IImageProviderService
    {
        Task<ImageResizeResult?> GetResizedAsync(long id, int updatedAtDate, int? width, int? height);

        FileItemData? GetOriginal(long id);
    }
}