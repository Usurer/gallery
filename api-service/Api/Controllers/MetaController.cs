using Api.Models;
using Core.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MetaController : ControllerBase
    {
        private IStorageQueryService StorageService;

        public MetaController(IStorageQueryService storageService)
        {
            StorageService = storageService;
        }

        [HttpGet("[action]/{folderId}")]
        public IResult FolderImages(long? folderId)
        {
            var result = StorageService.GetCollectionMetadata(folderId);
            return TypedResults.Ok(result);
        }

        [HttpGet("{id}")]
        public Results<ProblemHttpResult, Ok<ItemInfoModel>> Item(long id)
        {
            var result = StorageService.GetItem(id)?.ToItemModel();
            if (result == null)
            {
                return TypedResults.Problem(
                    title: "Item not found",
                    detail: "The requested item doesn't exist",
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            return TypedResults.Ok(result);
        }
    }
}