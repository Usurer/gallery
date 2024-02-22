using Core;

namespace Api.Services
{
    public interface IImageResizeService
    {
        Task<ImageResizeResult> GetAsync(FileItemData imageData, int? width, int? height);
    }
}