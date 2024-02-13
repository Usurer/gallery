using Core;
using Core.DTO;
using Core.Utils;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database
{
    internal class DatabaseStorageService : IStorageService
    {
        private readonly GalleryContext DbContext;

        private readonly ILogger<DatabaseStorageService> Logger;

        public DatabaseStorageService(
            GalleryContext dbContext,
            ILogger<DatabaseStorageService> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        public ItemInfo? GetItem(long id)
        {
            var item = DbContext
                .FileSystemItems
                .SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                Logger.LogWarning("Item with id {ItemId} was not found", id);
                return null;
            }

            if (item.IsFolder)
            {
                return new FolderItemInfo
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreationDate = DateTimeUtils.FromUnixTimestamp(item.CreationDate),
                    UpdatedAtDate = item.UpdatedAtDate,
                };
            }

            return new FileItemInfo
            {
                Id = item.Id,
                Name = item.Name,
                CreationDate = DateTimeUtils.FromUnixTimestamp(item.CreationDate),
                UpdatedAtDate = item.UpdatedAtDate,
                Width = item.Width.Value,
                Height = item.Height.Value,
                Extension = item.Extension,
            };
        }

        public async Task<IEnumerable<ItemInfo>> GetItemsAsync(int skip, int take)
        {
            IQueryable<FileSystemItem> items;

            items = DbContext.FileSystemItems;

            items = items
                .OrderBy(x => x.CreationDate)
                .Skip(skip)
                .Take(take);

            var result = new List<ItemInfo>();

            await foreach (var item in items.AsAsyncEnumerable())
            {
                ItemInfo newItem = item switch
                {
                    { IsFolder: true } => new FolderItemInfo
                    {
                        Id = item.Id,
                        Name = item.Name,
                        CreationDate = DateTimeUtils.FromUnixTimestamp(item.CreationDate),
                        UpdatedAtDate = item.UpdatedAtDate,
                    },
                    { IsFolder: false } => new FileItemInfo
                    {
                        Id = item.Id,
                        Name = item.Name,
                        CreationDate = DateTimeUtils.FromUnixTimestamp(item.CreationDate),
                        UpdatedAtDate = item.UpdatedAtDate,
                        Extension = item.Extension ?? string.Empty,
                        Height = item.Height.GetValueOrDefault(),
                        Width = item.Width.GetValueOrDefault(),
                    },
                };
            }

            return result;
        }

        public IEnumerable<FileItemInfo> GetFileItems(long? folderId, int skip, int take, string[]? extensions)
        {
            IQueryable<FileSystemItem> items;

            items = DbContext
                .FileSystemItems;

            if (folderId.HasValue)
            {
                items = items.Where(x => x.ParentId == folderId);
            }

            // TODO: Should we check whether the folder itself exist?
            items = items
                .Where(x => !x.IsFolder)
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.CreationDate)
                .Skip(skip)
                .Take(take);

            var result = new List<FileItemInfo>();
            foreach (var item in items)
            {
                if (extensions?.Length > 0)
                {
                    if (!extensions.Contains(item.Extension, StringComparer.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                }

                if (item.Name.EndsWith(".MP4", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                result.Add(new FileItemInfo
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreationDate = DateTimeUtils.FromUnixTimestamp(item.CreationDate),
                    UpdatedAtDate = item.UpdatedAtDate,

                    // TODO: I don't like the idea of nullable width and height
                    Width = item.Width.GetValueOrDefault(),
                    Height = item.Height.GetValueOrDefault(),
                    Extension = item.Extension,
                });
            }

            return result;
        }

        public IEnumerable<FolderItemInfo> GetFolderItems(long? folderId, int skip, int take)
        {
            IQueryable<FileSystemItem> items;

            items = DbContext.FileSystemItems;

            if (!folderId.HasValue)
            {
                items = items.Where(x => x.ParentId == null);
            }
            else
            {
                items = items.Where(x => x.ParentId == folderId);
            }

            // TODO: Should we query for the folder first and return error if there's no folder with given Id?
            items = items
                .Where(x => x.IsFolder)
                .OrderBy(x => x.CreationDate)
                .Skip(skip)
                .Take(take);

            var result = new List<FolderItemInfo>();
            foreach (var item in items)
            {
                result.Add(new FolderItemInfo
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreationDate = DateTimeUtils.FromUnixTimestamp(item.CreationDate),
                    UpdatedAtDate = item.UpdatedAtDate,
                });
            }

            return result;
        }

        public IEnumerable<FolderItemInfo>? GetFolderAncestors(long folderId)
        {
            var ansectors = new List<FileSystemItem>();
            var currentFolder = DbContext
                .FileSystemItems
                .SingleOrDefault(x => x.Id == folderId && x.IsFolder);

            if (currentFolder == null)
            {
                Logger.LogWarning("Folder with id {FolderId} was not found", folderId);
                return null;
            }

            ansectors.Add(currentFolder);

            var parent = DbContext
                .FileSystemItems
                .SingleOrDefault(x => x.Id == currentFolder.ParentId && x.IsFolder);

            while (parent != null)
            {
                ansectors.Add(parent);
                parent = DbContext
                    .FileSystemItems
                    .SingleOrDefault(x => x.Id == parent.ParentId && x.IsFolder);
            }

            return ansectors.Select(x =>
            {
                return new FolderItemInfo
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreationDate = DateTimeUtils.FromUnixTimestamp(x.CreationDate),
                    UpdatedAtDate = x.UpdatedAtDate,
                };
            });
        }

        public CollectionMetadata GetCollectionMetadata(long? rootId)
        {
            var result = new CollectionMetadata()
            {
                RootId = rootId,
                ItemsPerMonth = new Dictionary<DateTime, int>()
            };

            using var connection = DbContext.Database.GetDbConnection();
            connection.Open();

            // TODO: Refactor this, or at least add params properly
            var command = connection.CreateCommand();
            command.CommandText = $"" +
                $"SELECT count(id) as num, dateTime(CreationDate, 'unixepoch', 'start of day') as d " +
                $"FROM FileSystemItems " +
                $"WHERE isFolder = 0 ";

            if (rootId.HasValue)
            {
                command.CommandText += $"AND parentId = {rootId} ";
            }
            command.CommandText += $"GROUP BY d";

            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var count = reader.GetInt32(0);
                    var dateTimeString = reader.GetString(1);
                    // TODO: String format
                    var dateTime = DateTime.Parse(dateTimeString);
                    result.ItemsCount += count;
                    result.ItemsPerMonth.Add(dateTime, count);
                }
            }

            reader.Close();
            connection.Close();

            return result;
        }

        public FileItemData? GetImage(long id)
        {
            var fileItem = DbContext
                .FileSystemItems
                .SingleOrDefault(x => x.Id == id && x.IsFolder == false);

            if (fileItem == null)
            {
                Logger.LogWarning("Image with id {ImageId} was not found", id);
                return null;
            }

            /* From time to time I keep getting the System.IO.IOException:
             * The process cannot access the file because it is being used by another process.
             * Maybe I should just copy FileStream to a MemoryStream and release the file handler.
             * TODO: Fix this
             */
            var stream = new FileStream(fileItem.Path, FileMode.Open);
            var info = new FileItemData
            {
                Info = new FileItemInfo
                {
                    Id = fileItem.Id,
                    Name = fileItem.Name,
                    CreationDate = new DateTime(fileItem.CreationDate),
                    UpdatedAtDate = fileItem.UpdatedAtDate,
                    Extension = fileItem.Extension,
                    Width = fileItem.Width.Value,
                    Height = fileItem.Height.Value,
                },
                Data = stream
            };

            return info;
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

        // TODO: Find if there's a way to have a method that would accept the selector property,
        // i.e. instead of having 2 different GetByPath and GetById we would have
        // GetBy<T>(typeof(T) value) where T keyof FileSystemItem
        // There might be libraries for that
        public async Task<FileSystemItemDto?> GetByPathAsync(string path)
        {
            var item = await DbContext.FileSystemItems.FirstOrDefaultAsync(x => x.Path == path);
            return item?.ToDto();
        }

        public async Task<FileSystemItemDto> GetOrCreateFileSystemItemAsync(DirectoryInfo directoryInfo)
        {
            var rootDbRecord = DbContext.FileSystemItems.Where(x => x.Path == directoryInfo.FullName).FirstOrDefault();
            if (rootDbRecord == null)
            {
                rootDbRecord = directoryInfo.ToFileSystemItem(null, null, null);
                DbContext.FileSystemItems.Add(rootDbRecord);
                await DbContext.SaveChangesAsync();
            }
            return rootDbRecord.ToDto();
        }

        public async Task UpsertAsync(IEnumerable<FileSystemItemDto> add, IEnumerable<FileSystemItemDto> update)
        {
            var addTask = DbContext.FileSystemItems.AddRangeAsync(add.Select(x => x.ToEntity()));
            DbContext.FileSystemItems.UpdateRange(update.Select(x => x.ToEntity()));
            await addTask;

            await DbContext.SaveChangesAsync();
        }
    }
}