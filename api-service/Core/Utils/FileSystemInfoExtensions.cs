namespace Core.Utils
{
    public static class FileSystemInfoExtensions
    {
        public static bool IsDirectory(this FileSystemInfo info)
        {
            return info.Attributes.HasFlag(FileAttributes.Directory);
        }
    }
}