using Core.DTO;

namespace Database.Entities.Utils
{
    internal static class FileSystemItemExtensions
    {
        public static FileSystemItemDto ToDto(this FileSystemItem item)
        {
            return new FileSystemItemDto
            {
                Id = item.Id,
                Name = item.Name,
                CreationDate = item.CreationDate,
                Extension = item.Extension,
                Height = item.Height,
                IsFolder = item.IsFolder,
                ParentId = item.ParentId,
                Path = item.Path,
                UpdatedAtDate = item.UpdatedAtDate,
                Width = item.Width,
            };
        }
    }
}
