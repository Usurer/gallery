using Core.Abstractions;
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

        public async Task RemoveFolderFromScansAsync(long id)
        {
            var item = await DbContext.ScanTargets.Where(x => x.Id == id).SingleAsync();
            DbContext.ScanTargets.Remove(item);
            await DbContext.SaveChangesAsync();
        }

        public async Task<(long id, string path)?> GetScanTarget()
        {
            var item = await DbContext.ScanTargets.OrderBy(x => x.Id).FirstOrDefaultAsync();
            if (item != null)
            {
                return new(item.Id, item.Path);
            }
            return null;
        }

        public async Task<(long id, string path)?> GetScanTarget(long id)
        {
            var item = await DbContext.ScanTargets.SingleOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                return new(item.Id, item.Path);
            }
            return null;
        }
    }
}