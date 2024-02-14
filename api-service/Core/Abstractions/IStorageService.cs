using Core.DTO;

namespace Core.Abstractions
{
    public interface IStorageService
    {
        public Task<FileSystemItemDto> GetOrCreateFileSystemItemAsync(DirectoryInfo directoryInfo);

        public Task<FileSystemItemDto?> GetByPathAsync(string path);

        public Task UpsertAsync(IEnumerable<FileSystemItemDto> add, IEnumerable<FileSystemItemDto> update);
    }
}