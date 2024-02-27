using Core.Abstractions;
using Core.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageProviderService ImageProviderService;

        public ImagesController(IImageProviderService imageProviderService)
        {
            ImageProviderService = imageProviderService;
        }

        [HttpGet("[controller]/{id}")]
        [ResponseCache(Duration = 60)]
        public async Task<Results<NotFound, FileContentHttpResult>> Get(long id)
        {
            // imageData is disposable because of the Data stream, but FileStreamResult should take care of it
            var imageData = await ImageProviderService.GetOriginalAsync(id);

            if (imageData == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.File(imageData.Data, imageData.MimeType);
        }

        [HttpGet("[controller]/{id}/[action]")]
        [ResponseCache(Duration = 3600 * 24)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<Results<ProblemHttpResult, FileContentHttpResult>> Preview(
            long id, int timestamp, int? width, int? height
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

            var result = await ImageProviderService.GetResizedAsync(id, timestamp, width, height);
            if (result == null)
            {
                return TypedResults.Problem(

                    statusCode: StatusCodes.Status404NotFound,
                    title: "Image not found"
                );
            }

            return TypedResults.File(
                result.Data,
                contentType: result.MimeType
            );
        }
    }
}