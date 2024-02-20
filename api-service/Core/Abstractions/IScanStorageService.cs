using Core.DTO;

namespace Core.Abstractions
{
    public interface IScanStorageService
    {
        public Task<long> AddFolderToScansAsync(string path);

        public Task RemoveFolderFromScansAsync(long id);

        public Task UpdateScanTarget(ScanTargetDto item);

        public Task<ScanTargetDto[]> GetAll();

        public Task<ScanTargetDto?> GetScanTarget(bool ignoreScanned = true);

        public Task<ScanTargetDto?> GetScanTarget(long id, bool ignoreScanned = true);
    }
}