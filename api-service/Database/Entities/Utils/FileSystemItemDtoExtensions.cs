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

            if (item is FileItemDto fileDto)
            {
                var image = new Image
                {
                    Width = fileDto.Width,
                    Height = fileDto.Height,
                    Extension = fileDto.Extension,
                    FileSystemItem = fileSystemItem
                };

                fileSystemItem.Image = image;
            }

            return fileSystemItem;
        }
    }
}