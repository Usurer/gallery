using Api.Models;
using Core.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FoldersController : ControllerBase
    {
        private readonly IStorageQueryService StorageService;

        public FoldersController(IStorageQueryService storageService)
        {
            StorageService = storageService;
        }

        [HttpGet("/[controller]/{folderId}")]
        [HttpGet("/[controller]")]
        public IEnumerable<FolderItemInfoModel> Get(long? folderId, int skip = 0, int take = 10)
        {
            return StorageService.GetFolderItems(folderId, skip, take).Select(x => x.ToFolderModel());
        }

        [HttpGet("{folderId}")]
        public IEnumerable<FileItemInfoModel> Files(long? folderId, int skip = 0, int take = 10, [FromQuery] string[]? extensions = null)
        {
            return StorageService.GetFileItems(folderId, skip, take, extensions).Select(x => x.ToFileModel());
        }

        [HttpGet("{folderId}")]
        public Results<Ok<IEnumerable<FolderItemInfoModel>>, ProblemHttpResult> Ancestors(long folderId)
        {
            var result = StorageService.GetFolderAncestors(folderId);
            if (result == null)
            {
                return TypedResults.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Folder not found"
                );
            }

            return TypedResults.Ok(result.Select(x => x.ToFolderModel()));
        }
    }
}