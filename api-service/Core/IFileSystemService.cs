using Core;

namespace Core
{
    public interface IFileSystemService
    {
        public Task<ScanFolderResult> ScanFolderAsync(string? fullPath, IProgress<int>? progress);

        public IAsyncEnumerable<ScanFolderResult> ScanFoldersFromRootAsync(string? root);
    }
}