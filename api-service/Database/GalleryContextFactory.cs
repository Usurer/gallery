using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database;

internal class GalleryContextFactory : IDesignTimeDbContextFactory<GalleryContext>
{
    public GalleryContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GalleryContext>();
        optionsBuilder.UseSqlite();

        return new GalleryContext(optionsBuilder.Options);
    }
}