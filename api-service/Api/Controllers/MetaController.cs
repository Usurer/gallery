using Api.Models;
using Core.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MetaController : ControllerBase
    {
        private IStorageQueryService StorageService;

        public MetaController(IStorageQueryService storageService)
        {
            StorageService = storageService;
        }

        [HttpGet]
        public IResult FolderImages(long? parentId)
        {
            var result = StorageService.GetCollectionMetadata(parentId);
            return TypedResults.Ok(result);
        }

        [HttpGet]
        public Results<ProblemHttpResult, Ok<ItemInfoModel>> Item([BindRequired] long id)
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