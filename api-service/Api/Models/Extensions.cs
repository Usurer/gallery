using Core.DTO;
using Core.Utils;

namespace Api.Models
{
    public static class Extensions
    {
        public static FolderItemInfoModel ToFolderModel(this FileSystemItemDto dto)
        {
            if (dto is FolderItemDto folder)
            {
                return new FolderItemInfoModel
                {
                    Id = folder.Id,
                    Name = folder.Name,
                    CreationDate = DateTimeUtils.FromUnixTimestamp(folder.CreationDate),
                    UpdatedAtDate = folder.UpdatedAtDate,
                };
            }

            throw new InvalidOperationException($"Cannot transform file id={dto.Id} to folder item");
        }

        public static FileItemInfoModel ToFileModel(this FileSystemItemDto dto)
        {
            if (dto is FileItemDto file)
            {
                return new FileItemInfoModel
                {
                    Id = file.Id,
                    Name = file.Name,
                    CreationDate = DateTimeUtils.FromUnixTimestamp(file.CreationDate),
                    UpdatedAtDate = file.UpdatedAtDate,
                    Width = file.Width,
                    Height = file.Height,
                    Extension = file.Extension,
                };
            }

            throw new InvalidOperationException($"Cannot transform folder id={dto.Id} to file item");
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