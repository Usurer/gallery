using Core.DTO;

namespace Database.Entities.Utils
{
    internal static class FileSystemItemExtensions
    {
        public static FileItemDto ToFileDto(this FileSystemItem item, Image image)
        {
            return new FileItemDto
            {
                Id = item.Id,
                Name = item.Name,
                CreationDate = item.CreationDate,
                Extension = image.Extension,
                Height = image.Height,
                ParentId = item.ParentId,
                Path = item.Path,
                UpdatedAtDate = item.UpdatedAtDate,
                Width = image.Width,
            };
        }

        public static FolderItemDto ToFolderDto(this FileSystemItem item)
        {
            return new FolderItemDto
            {
                Id = item.Id,
                Name = item.Name,
                CreationDate = item.CreationDate,
                ParentId = item.ParentId,
                Path = item.Path,
                UpdatedAtDate = item.UpdatedAtDate,
            };
        }

        public static FileSystemItemDto ToDto(this FileSystemItem item)
        {
            return item.IsFolder ? item.ToFolderDto() : item.ToFileDto(item.Image);
        }
    }
}