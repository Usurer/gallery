using Api.Models;
using Core.Abstractions;
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
        private readonly IStorageQueryService StorageQueryService;

        public DatabaseController(ILogger<DatabaseController> logger, IStorageService storageService, IStorageQueryService storageQueryService)
        {
            Logger = logger;
            StorageService = storageService;
            StorageQueryService = storageQueryService;
        }

        [HttpGet]
        public IEnumerable<ItemInfoModel> Get(int take = 100, int skip = 0)
        {
            var item = StorageQueryService.GetItems(skip, take).Select(x => x.ToItemModel());
            return item;
        }

        [HttpDelete]
        public async Task<IResult> Purge()
        {
            await StorageService.Purge();
            return Results.Ok();
        }
    }
}