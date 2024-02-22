namespace Core.Abstractions
{
    internal interface IImageResizeService
    {
        Task<ImageResizeResult> GetAsync(FileItemData imageData, int? width, int? height);
    }
}