using Core;
using Core.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Internal
{
    /// <summary>
    /// Test controller for direct access to the DbContext
    /// </summary>
    [ApiController]
    [Route("internals/[controller]/[action]")]
    public class DatabaseController : ControllerBase
    {
        private readonly ILogger<DatabaseController> Logger;
        private readonly IStorageService StorageService;

        public DatabaseController(ILogger<DatabaseController> logger, IStorageService storageService)
        {
            Logger = logger;
            StorageService = storageService;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemInfo>> Get(int take = 100, int skip = 0)
        {
            var item = await StorageService.GetItemsAsync(skip, take);
            return item;
        }

        [HttpPut]
        public async Task<IResult> Put()
        {
            var item = new FileSystemItemDto
            {
                Path = "Some path",
            };
            
            await StorageService.UpsertAsync([item], Enumerable.Empty<FileSystemItemDto>());

            return Results.Ok();
        }
    }
}