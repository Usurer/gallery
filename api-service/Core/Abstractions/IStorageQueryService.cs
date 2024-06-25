using Core.DTO;

namespace Core.Abstractions
{
    public interface IStorageQueryService
    {
        public FileSystemItemDto? GetItem(long id);

        public IEnumerable<FileSystemItemDto> GetItems(int skip, int take);

        public IEnumerable<FileItemDto> GetFileItems(long? folderId, int skip, int take, string[]? extensions);

        public IEnumerable<FolderItemDto> GetFolderItems(long? folderId, int skip, int take);

        public IEnumerable<FolderItemDto>? GetFolderAncestors(long folderId);

        public CollectionMetadataDto GetCollectionMetadata(long? rootId);
    }
}