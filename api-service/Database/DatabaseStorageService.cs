using Core;
using Core.Abstractions;
using Core.DTO;
using Database.Entities.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database
{
    internal class DatabaseStorageService : IStorageQueryService, IStorageService
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

        public FileSystemItemDto? GetItem(long id)
        {
            var item = DbContext
                .FileSystemItems
                .SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                Logger.LogWarning("Item with id {ItemId} was not found", id);
                return null;
            }

            return item.ToDto();
        }

        public IEnumerable<FileSystemItemDto> GetItems(int skip, int take)
        {
            IQueryable<FileSystemItem> items;

            items = DbContext.FileSystemItems;

            items = items
                .OrderBy(x => x.CreationDate)
                .Skip(skip)
                .Take(take);

            var result = new List<FileSystemItemDto>();

            return items.Select(x => x.ToDto()).ToArray();
        }

        public IEnumerable<FileSystemItemDto> GetFileItems(long? folderId, int skip, int take, string[]? extensions)
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

            var result = new List<FileSystemItemDto>();

            //TODO: can it be rewritten as lambda?
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

                result.Add(item.ToDto());
            }

            return result;
        }

        public IEnumerable<FileSystemItemDto> GetFolderItems(long? folderId, int skip, int take)
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

            return items.Select(x => x.ToDto()).ToArray();
        }

        public IEnumerable<FileSystemItemDto>? GetFolderAncestors(long folderId)
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

            return ansectors.Select(x => x.ToDto()).ToArray();
        }

        public CollectionMetadataDto GetCollectionMetadata(long? rootId)
        {
            var result = new CollectionMetadataDto()
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
                Info = fileItem.ToDto(),
                Data = stream
            };

            return info;
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