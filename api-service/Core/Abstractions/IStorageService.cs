using Core.DTO;
using Core.Models;

namespace Core.Abstractions
{
    public interface IStorageService
    {
        public ItemInfoModel? GetItem(long id);

        public Task<IEnumerable<ItemInfoModel>> GetItemsAsync(int skip, int take);

        public IEnumerable<FileItemInfoModel> GetFileItems(long? folderId, int skip, int take, string[]? extensions);

        public IEnumerable<FolderItemInfoModel> GetFolderItems(long? folderId, int skip, int take);

        public IEnumerable<FolderItemInfoModel>? GetFolderAncestors(long folderId);

        public CollectionMetadataModel GetCollectionMetadata(long? rootId);

        public FileItemData? GetImage(long id);

        // TODO: Next should be extracted into separate Scans service
        // Plus there should be a method to add a new Scan
        public Task RemoveFolderFromScansAsync(long id);

        public Task<(long id, string path)?> GetScanTarget();

        public Task<(long id, string path)?> GetScanTarget(long id);

        public Task<long> AddFolderToScansAsync(string path);


        // TODO: Extract next into a separate service

        public Task<FileSystemItemDto> GetOrCreateFileSystemItemAsync(DirectoryInfo directoryInfo);

        public Task<FileSystemItemDto?> GetByPathAsync(string path);

        public Task UpsertAsync(IEnumerable<FileSystemItemDto> add, IEnumerable<FileSystemItemDto> update);

    }
}