using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests.Integration")]

namespace Database;

internal class GalleryContext : DbContext, IGalleryContext
{
    public GalleryContext(DbContextOptions<GalleryContext> options) : base(options)
    {
    }

    public virtual DbSet<FileSystemItem> FileSystemItems
    {
        get; set;
    }

    public virtual DbSet<ScanTarget> ScanTargets
    {
        get; set;
    }

    public virtual DbSet<Image> Images
    {
        get; set;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileSystemItem>(entity =>
        {
            entity.ToTable("FileSystemItems");

            entity.Property(e => e.Id).HasColumnType("INTEGER").IsRequired();

            entity.HasKey(e => e.Id);

            entity
                .HasOne(e => e.Parent)
                .WithMany()
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable(nameof(Images));

            entity.HasKey(e => e.FileSystemItemId);

            entity
                .HasOne(e => e.FileSystemItem)
                .WithOne(e => e.Image)
                .HasForeignKey<Image>(e => e.FileSystemItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        });

        modelBuilder.Entity<ScanTarget>(entity =>
        {
            entity.ToTable(nameof(ScanTargets));

            entity.Property(e => e.Id).HasColumnType("INTEGER").IsRequired();
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Path);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    private void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
    }
}