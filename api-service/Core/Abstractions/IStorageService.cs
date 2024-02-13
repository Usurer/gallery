using Core.DTO;

namespace Core.Abstractions
{
    public interface IStorageService
    {
        public FileSystemItemDto? GetItem(long id);

        public IEnumerable<FileSystemItemDto> GetItems(int skip, int take);

        public IEnumerable<FileSystemItemDto> GetFileItems(long? folderId, int skip, int take, string[]? extensions);

        public IEnumerable<FileSystemItemDto> GetFolderItems(long? folderId, int skip, int take);

        public IEnumerable<FileSystemItemDto>? GetFolderAncestors(long folderId);

        public CollectionMetadataDto GetCollectionMetadata(long? rootId);

        public FileItemData? GetImage(long id);


        // TODO: Extract next into a separate service

        public Task<FileSystemItemDto> GetOrCreateFileSystemItemAsync(DirectoryInfo directoryInfo);

        public Task<FileSystemItemDto?> GetByPathAsync(string path);

        public Task UpsertAsync(IEnumerable<FileSystemItemDto> add, IEnumerable<FileSystemItemDto> update);

    }
}