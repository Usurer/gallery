using Core.DTO;

namespace Database.Entities.Utils
{
    internal static class FileSystemItemDtoExtensions
    {
        public static FileSystemItem ToEntity(this FileSystemItemDto item)
        {
            var fileSystemItem = new FileSystemItem
            {
                Id = item.Id,
                Name = item.Name,
                CreationDate = item.CreationDate,
                IsFolder = item.IsFolder,
                ParentId = item.ParentId,
                Path = item.Path,
                UpdatedAtDate = item.UpdatedAtDate,
            };

            if (!item.IsFolder)
            {
                var image = new Image
                {
                    Width = item.Width ?? 0, // TODO: Refactor these
                    Height = item.Height ?? 0,
                    Extension = item.Extension ?? string.Empty,
                    FileSystemItem = fileSystemItem
                };

                fileSystemItem.Image = image;
            }

            return fileSystemItem;
        }
    }
}