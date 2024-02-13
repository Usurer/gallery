using Core;

namespace Database
{
    internal static class FileSystemItemDtoExtensions
    {

        public static FileSystemItem ToEntity(this FileSystemItemDto item)
        {
            return new FileSystemItem
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