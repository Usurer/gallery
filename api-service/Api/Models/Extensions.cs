using Core.DTO;
using Core.Utils;

namespace Api.Models
{
    public static class Extensions
    {
        public static FolderItemInfoModel ToFolderModel(this FileSystemItemDto dto)
        {
            if (!dto.IsFolder)
            {
                throw new InvalidOperationException($"Cannot transform file id={dto.Id} to folder item");
            }

            return new FolderItemInfoModel
            {
                Id = dto.Id,
                Name = dto.Name,
                CreationDate = DateTimeUtils.FromUnixTimestamp(dto.CreationDate),
                UpdatedAtDate = dto.UpdatedAtDate,
            };
        }

        public static FileItemInfoModel ToFileModel(this FileSystemItemDto dto)
        {
            if (dto.IsFolder)
            {
                throw new InvalidOperationException($"Cannot transform folder id={dto.Id} to file item");
            }

            return new FileItemInfoModel
            {
                Id = dto.Id,
                Name = dto.Name,
                CreationDate = DateTimeUtils.FromUnixTimestamp(dto.CreationDate),
                UpdatedAtDate = dto.UpdatedAtDate,
                Width = dto.Width.GetValueOrDefault(),
                Height = dto.Height.GetValueOrDefault(),
                Extension = dto.Extension ?? string.Empty,
            };
        }

        public static ItemInfoModel ToItemModel(this FileSystemItemDto dto)
        {
            if (dto.IsFolder)
            {
                return dto.ToFolderModel();
            }

            return dto.ToFileModel();
        }
    }
}
