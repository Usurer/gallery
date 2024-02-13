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
        private readonly IStorageService _storageService;

        public FoldersController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet("{folderId}")]
        [HttpGet()]
        public IEnumerable<FolderItemInfoModel> ListItems(long? folderId, int skip = 0, int take = 10)
        {
            return _storageService.GetFolderItems(folderId, skip, take).Select(x => x.ToFolderModel());
        }

        [HttpGet("{folderId}")]
        public Results<Ok<IEnumerable<FolderItemInfoModel>>, ProblemHttpResult> GetAncestors(long folderId)
        {
            var result = _storageService.GetFolderAncestors(folderId);
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