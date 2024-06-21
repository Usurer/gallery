using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class ImageNoCascadeOnPrincipalDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_FileSystemItems_FileSystemItemId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_FileSystemItems_FileSystemItemId",
                table: "Images",
                column: "FileSystemItemId",
                principalTable: "FileSystemItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_FileSystemItems_FileSystemItemId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_FileSystemItems_FileSystemItemId",
                table: "Images",
                column: "FileSystemItemId",
                principalTable: "FileSystemItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
