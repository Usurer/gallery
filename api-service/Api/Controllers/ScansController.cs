﻿using Api.Services;
using Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ScansController : ControllerBase
    {
        private readonly IStorageService storageService;
        private readonly IScansProcessingService ScansProcessingService;

        public ScansController(IStorageService storageService, IScansProcessingService scansProcessingService)
        {
            this.storageService = storageService;
            ScansProcessingService = scansProcessingService;
        }

        [HttpPut]
        public async Task<Ok> AddScan([FromBody] string path)
        {
            var id = await storageService.AddFolderToScansAsync(path);

            // TODO: Figure out the proper way to do error handling here
            _ = ScansProcessingService.EnqueueNextScanAsync(id);
            return TypedResults.Ok();
        }
    }
}