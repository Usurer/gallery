using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageSpecificPropsFromFileSystemItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "FileSystemItems");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "FileSystemItems");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "FileSystemItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "FileSystemItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "FileSystemItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "FileSystemItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE FileSystemItems
                SET Height = t2.Height, Width = t2.Width, Extension = t2.Extension
                FROM Images t2
                WHERE t2.FileSystemItemId = FileSystemItems.Id
            ");
        }
    }
}