using Core.DTO;

namespace Core.Abstractions
{
    public interface IStorageService
    {
        public Task<FolderItemDto> GetOrCreateFolderItemAsync(FolderItemDto dto);

        public Task<FileSystemItemDto?> GetByPathAsync(string path);

        public Task UpsertAsync(IEnumerable<FileSystemItemDto> add, IEnumerable<FileSystemItemDto> update);

        public Task Purge();
    }
}