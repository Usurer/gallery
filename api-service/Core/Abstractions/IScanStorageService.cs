namespace Core.Abstractions
{
    public interface IScanStorageService
    {
        public Task<long> AddFolderToScansAsync(string path);

        public Task RemoveFolderFromScansAsync(long id);

        public Task<(long id, string path)?> GetScanTarget();

        public Task<(long id, string path)?> GetScanTarget(long id);
    }
}