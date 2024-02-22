using Core.Abstractions;
using Core.Utils;
using Imageflow.Fluent;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    internal class ImageResizeService : IImageResizeService
    {
        private readonly ILogger<ImageResizeService> Logger;

        public ImageResizeService(ILogger<ImageResizeService> logger)
        {
            Logger = logger;
        }

        // TODO: Can we include UpdatedAtDate into a cache key? Would be really nice for handling updated items
        public async Task<ImageResizeResult> GetAsync(FileItemData imageData, int? width, int? height)
        {
            var widthParam = width.HasValue ? $"width={width}" : string.Empty;
            var heightParam = height.HasValue ? $"height={height}" : string.Empty;
            var resizeParam = string.Join("&", new[] { widthParam, heightParam }.Where(x => !string.IsNullOrEmpty(x)));

            MemoryStream resizedStream = new MemoryStream();
            var job = new ImageJob();
            var resizeResult = await job.Decode(imageData.Data, true)
                .ResizerCommands($"{resizeParam}&mode=crop")
                // TODO: Set disposeUnderlying to true?
                .Encode(new StreamDestination(resizedStream, false), new PngQuantEncoder())
                .Finish()
                .InProcessAsync();

            var data = resizedStream.ToArray();
            var mime = MimeUtils.ExtensionToMime(imageData.Info.Extension);
            var result = new ImageResizeResult
            {
                Data = data,
                MimeType = mime,
                Name = imageData.Info.Name
            };
            return result;
        }
    }
}