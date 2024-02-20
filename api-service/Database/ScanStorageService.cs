using Core.Abstractions;
using Core.DTO;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database
{
    internal class ScanStorageService : IScanStorageService
    {
        private readonly GalleryContext DbContext;

        private readonly ILogger<ScanStorageService> Logger;

        public ScanStorageService(
            GalleryContext dbContext,
            ILogger<ScanStorageService> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        public async Task<long> AddFolderToScansAsync(string path)
        {
            var existingItem = DbContext.FileSystemItems.Where(x => x.Path == path).SingleOrDefault();
            if (existingItem != null)
            {
                throw new ApplicationException($"{path} is already in DB");
            }

            var entity = new ScanTarget
            {
                Path = path
            };
            DbContext.ScanTargets.Add(entity);

            await DbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateScanTarget(ScanTargetDto item)
        {
            var entity = await DbContext.ScanTargets.Where(x => x.Id == item.Id).AsTracking().SingleAsync();
            entity.IsScanned = item.IsScanned;
            entity.IsInvalid = item.IsInvalid;
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveFolderFromScansAsync(long id)
        {
            var item = await DbContext.ScanTargets.Where(x => x.Id == id).SingleAsync();
            DbContext.ScanTargets.Remove(item);
            await DbContext.SaveChangesAsync();
        }

        public async Task<ScanTargetDto[]> GetAll()
        {
            return await DbContext
                .ScanTargets
                .OrderBy(x => x.Id)
                .Select(x => new ScanTargetDto { Id = x.Id, Path = x.Path, IsScanned = x.IsScanned })
                .ToArrayAsync();
        }

        public async Task<ScanTargetDto?> GetScanTarget(bool ignoreScanned = true)
        {
            var item = await DbContext
                .ScanTargets
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x => ignoreScanned ? x.IsScanned == false : true);

            if (item != null)
            {
                return new ScanTargetDto { Id = item.Id, Path = item.Path, IsScanned = item.IsScanned };
            }

            Logger.LogInformation("Not found next ScanTarget, scanned items ignored {@IgnoreScanned}", ignoreScanned);
            return null;
        }

        public async Task<ScanTargetDto?> GetScanTarget(long id, bool ignoreScanned = true)
        {
            var item = await DbContext
                .ScanTargets
                .SingleOrDefaultAsync(x => x.Id == id && (ignoreScanned ? x.IsScanned == false : true));

            if (item != null)
            {
                return new ScanTargetDto { Id = item.Id, Path = item.Path, IsScanned = item.IsScanned };
            }

            Logger.LogInformation("Not found ScanTarget {@Id}, scanned items ignored {@IgnoreScanned}", id, ignoreScanned);
            return null;
        }
    }
}