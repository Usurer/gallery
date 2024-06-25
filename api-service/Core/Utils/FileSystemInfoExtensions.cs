using Core.DTO;
using System.IO;

namespace Core.Utils
{
    public static class FileSystemInfoExtensions
    {
        public static bool IsDirectory(this FileSystemInfo info)
        {
            return info.Attributes.HasFlag(FileAttributes.Directory);
        }

        public static FileItemDto ToFileItemDto(
            this FileSystemInfo info,
            long parentId,
            int width,
            int height
            )
        {
            var creationDate = GetCreationDate(info);

            return new FileItemDto
            {
                Path = info.FullName,
                Name = info.Name,
                Extension = info.Extension,
                CreationDate = DateTimeUtils.ToUnixTimestamp(creationDate),
                UpdatedAtDate = DateTimeUtils.ToUnixTimestamp(DateTime.Now),
                ParentId = parentId,
                Width = width,
                Height = height,
            };
        }

        public static FolderItemDto ToFolderItemDto(
            this FileSystemInfo info,
            long? parentId
            )
        {
            var creationDate = GetCreationDate(info);

            return new FolderItemDto
            {
                Path = info.FullName,
                Name = info.Name,
                CreationDate = DateTimeUtils.ToUnixTimestamp(creationDate),
                UpdatedAtDate = DateTimeUtils.ToUnixTimestamp(DateTime.Now),
                ParentId = parentId,
            };
        }

        private static DateTime GetCreationDate(FileSystemInfo info)
        {
            // Some files have Created date which is later than Modified date
            // Probably due to file copying
            // OFC I should use date from EXIF, but right now let's just do like that
            return info.CreationTime > info.LastWriteTime
                        ? info.LastWriteTime
                        : info.CreationTime;
        }
    }
}