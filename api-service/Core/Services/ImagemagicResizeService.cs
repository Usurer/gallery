using Core.Abstractions;
using Core.Utils;
using ImageMagick;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Services
{
    internal class ImagemagicResizeService : IImageResizeService
    {
        private readonly ILogger<ImagemagicResizeService> Logger;

        public ImagemagicResizeService(ILogger<ImagemagicResizeService> logger)
        {
            Logger = logger;
        }

        // TODO: Can we include UpdatedAtDate into a cache key? Would be really nice for handling updated items
        public async Task<ImageResizeResult> GetAsync(FileItemData imageData, int? width, int? height)
        {
            var geometry = new MagickGeometryFactory().Create();
            if (width.HasValue)
            {
                geometry.Width = width.Value;
            }
            if (height.HasValue)
            {
                geometry.Height = height.Value;
            }

            MemoryStream resizedStream = new MemoryStream();

            var format = imageData.Info.Extension.ToLower() switch
            {
                ".arw" => MagickFormat.Arw,
                ".cr2" => MagickFormat.Cr2,
                ".bmp" => MagickFormat.Bmp,
                _ => MagickFormat.Raw
            };

            byte[] data;

            using var imageFromFile = new MagickImage(imageData.Data, format);
            var thumbnailData = imageFromFile.GetProfile("dng:thumbnail")?.GetData();
            if (thumbnailData != null)
            {
                using var thumbnail = new MagickImage(thumbnailData);
                thumbnail.Resize(geometry);
                await imageFromFile.WriteAsync(resizedStream, MagickFormat.Jpg);
                data = resizedStream.ToArray();
            }
            else
            {
                imageFromFile.Resize(geometry);
                await imageFromFile.WriteAsync(resizedStream, MagickFormat.Jpg);
                data = resizedStream.ToArray();
            }

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