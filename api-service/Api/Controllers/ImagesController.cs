using Api.Models;
using Api.Services;
using Api.Utils;
using Core.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers
{
    /// <summary>
    /// List images, get image by ID
    /// </summary>
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IFileSystemService FileSystemService;

        private readonly ImageResizeService ResizeService;

        public ImagesController(ImageResizeService resizeService, IFileSystemService fileSystemService)
        {
            ResizeService = resizeService;
            FileSystemService = fileSystemService;
        }

        [HttpGet("[controller]/{id}")]
        [ResponseCache(Duration = 60)]
        public Results<NotFound, FileStreamHttpResult> Get([BindRequired] long id)
        {
            // imageData is disposable because of the Data stream, but FileStreamResult should take care of it
            var imageData = FileSystemService.GetImage(id);

            if (imageData == null)
            {
                return TypedResults.NotFound();
            }

            var mime = MimeUtils.ExtensionToMime(imageData.Info.Extension);

            return TypedResults.File(imageData.Data, mime, imageData.Info.Name);
        }

        [HttpGet("[controller]/{id}/[action]")]
        [ResponseCache(Duration = 3600 * 24)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<Results<ProblemHttpResult, FileContentHttpResult>> Preview(
            [BindRequired] long id, int timestamp, int? width, int? height
        )
        {
            if (width == null && height == null)
            {
                return TypedResults.Problem(

                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Missing parameters",
                    detail: "Either width or height should be provided"
                );
            }

            var result = await ResizeService.GetAsync(id, timestamp, width, height);
            if (result == null)
            {
                return TypedResults.Problem(

                    statusCode: StatusCodes.Status404NotFound,
                    title: "Image not found"
                );
            }

            return TypedResults.File(result.Data, contentType: result.MimeType);
        }
    }
}