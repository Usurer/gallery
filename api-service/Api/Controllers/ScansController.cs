using Api.Services;
using Core.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScansController : ControllerBase
    {
        private readonly IScanStorageService storageService;
        private readonly IScansProcessingService ScansProcessingService;

        public ScansController(IScanStorageService storageService, IScansProcessingService scansProcessingService)
        {
            this.storageService = storageService;
            ScansProcessingService = scansProcessingService;
        }

        [HttpPut]
        public async Task<Ok> Put([FromBody] string path)
        {
            var id = await storageService.AddFolderToScansAsync(path);

            // TODO: Figure out the proper way to do error handling here
            _ = ScansProcessingService.EnqueueNextScanAsync(id);
            return TypedResults.Ok();
        }
    }
}