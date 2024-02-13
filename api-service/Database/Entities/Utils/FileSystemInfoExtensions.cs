using Core.Utils;

namespace Database.Entities.Utils
{
    internal static class FileSystemInfoExtensions
    {
        public static FileSystemItem ToFileSystemItem(
            this FileSystemInfo fileSystemInfo,
            long? parentid,
            int? width,
            int? height
            )
        {
            // Some files have Created date which is later than Modified date
            // Probably due to file copying
            // OFC I should use date from EXIF, but right now let's just do like that
            var creationDate = fileSystemInfo.CreationTime > fileSystemInfo.LastWriteTime
                        ? fileSystemInfo.LastWriteTime
                        : fileSystemInfo.CreationTime;

            return new FileSystemItem
            {
                Path = fileSystemInfo.FullName,
                Name = fileSystemInfo.Name,
                IsFolder = fileSystemInfo.IsDirectory(),
                Extension = fileSystemInfo.Extension,
                CreationDate = DateTimeUtils.ToUnixTimestamp(creationDate),
                UpdatedAtDate = DateTimeUtils.ToUnixTimestamp(DateTime.Now),
                ParentId = parentid,
                Width = width,
                Height = height,
            };
        }
    }
}