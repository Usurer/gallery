using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public interface IGalleryContext
    {
        public DbSet<FileSystemItem> FileSystemItems
        {
            get; set;
        }

        public DbSet<ScanTarget> ScanTargets
        {
            get; set;
        }
    }
}