using Core;
using Core.Abstractions;
using Core.DTO;
using Core.Utils;
using Imageflow.Bindings;
using Imageflow.Fluent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileSystem
{
    /*
     * TODO: Refactor it
     * I don't like how it's implemented right now - it's not clear that this class writes to DB
     * Maybe it should only read FS data and then use IStorageService implementation to write it
     * */

    public class FileSystemService : IFileSystemService
    {
        private readonly FileSystemOptions FileSystemOptions;

        private readonly IStorageService StorageService;

        private readonly IStorageQueryService StorageQueryService;

        private readonly ILogger<FileSystemService> Logger;

        public FileSystemService(IOptions<FileSystemOptions> options, IStorageService storageService, ILogger<FileSystemService> logger, IStorageQueryService storageQueryService)
        {
            FileSystemOptions = options.Value;
            StorageService = storageService;
            Logger = logger;
            StorageQueryService = storageQueryService;
        }

        public async IAsyncEnumerable<ScanFolderResult> ScanFoldersFromRootAsync(string? root)
        {
            root = string.IsNullOrEmpty(root) ? FileSystemOptions.DefaultFolder : root.Trim();

            if (!Directory.Exists(root))
            {
                throw new ApplicationException($"Path {root} doesn't exist");
            }

            var rootDirectoryInfo = new DirectoryInfo(root);
            var directories = rootDirectoryInfo.GetDirectories();

            var folderResult = await ScanFolderAsync(root);
            //results.Add(folderResult);
            yield return folderResult;

            foreach (var directory in directories)
            {
                var subtreeResults = ScanFoldersFromRootAsync(directory.FullName);
                await foreach (var subtreeResult in subtreeResults)
                {
                    yield return subtreeResult;
                }
            }

            yield break;
        }

        public async Task<ScanFolderResult> ScanFolderAsync(string? fullPath, IProgress<int>? progress = null)
        {
            fullPath = string.IsNullOrEmpty(fullPath) ? FileSystemOptions.DefaultFolder : fullPath.Trim();
            var result = new ScanFolderResult { Path = fullPath };

            if (Directory.Exists(fullPath))
            {
                var batchSize = 100;
                var batchCounter = 0;

                var rootDirectoryInfo = new DirectoryInfo(fullPath);

                var dto = rootDirectoryInfo.ToFolderItemDto(null);
                var rootDbRecord = await StorageService.GetOrCreateFolderItemAsync(dto);

                var fileSystemInfos = rootDirectoryInfo.EnumerateFileSystemInfos();

                var batch = fileSystemInfos.Skip(batchCounter * batchSize).Take(batchSize).ToArray();

                var newItems = new List<FileSystemItemDto>();
                var updatedItems = new List<FileSystemItemDto>();

                while (batch.Length > 0)
                {
                    for (int i = 0; i < batch.Length; i++)
                    {
                        var fileSystemInfo = batch[i];
                        var isDirectory = fileSystemInfo.IsDirectory();

                        var dbItem = await StorageService.GetByPathAsync(fileSystemInfo.FullName);
                        var existsInDb = dbItem != null;

                        // TODO: Even if existsInDb we can update missing ParentId if it's possible. Not sure about it
                        if (!existsInDb)
                        {
                            // TODO: Extract all image processing into a namespace under a Core namespace
                            ImageInfo? imageInfo = null;

                            if (!isDirectory)
                            {
                                using var imageData = File.OpenRead(fileSystemInfo.FullName);
                                try
                                {
                                    imageInfo = await ImageJob.GetImageInfo(new StreamSource(imageData, false));
                                    var newItem = fileSystemInfo.ToFileItemDto(
                                        rootDbRecord.Id,
                                        (int)imageInfo.ImageWidth,
                                        (int)imageInfo.ImageHeight
                                    );
                                    newItems.Add(newItem);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError(ex, "Error while getting Image Info for {FileName}", fileSystemInfo.FullName);
                                }
                            }
                            else
                            {
                                var newItem = fileSystemInfo.ToFolderItemDto(rootDbRecord.Id);
                                newItems.Add(newItem);
                            }

                            // Yeah, these are not saved yet, but okay
                            result.Saved++;
                        }
                        else
                        {
                            if (dbItem.ParentId == null)
                            {
                                dbItem.ParentId = rootDbRecord.Id;

                                updatedItems.Add(dbItem);
                            }
                        }
                        result.Total++;
                    }

                    await StorageService.UpsertAsync(newItems, updatedItems);
                    newItems.Clear();
                    updatedItems.Clear();

                    if (progress != null)
                    {
                        progress.Report(batchSize * batchCounter + batch.Length);
                    }

                    batchCounter++;

                    batch = fileSystemInfos.Skip(batchCounter * batchSize).Take(batchSize).ToArray();
                }
            }

            return result;
        }

        public FileItemData? GetImage(long id)
        {
            var item = StorageQueryService.GetItem(id);
            if (item is FileItemDto fileItem && fileItem is not null)
            {
                var stream = new FileStream(fileItem.Path, FileMode.Open);
                var info = new FileItemData
                {
                    Info = fileItem,
                    Data = stream
                };

                return info;
            }

            Logger.LogWarning("Image with id {ImageId} was not found in the database", id);
            return null;
        }
    }
}