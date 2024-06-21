using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class FileSystemItemsSelfReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FileSystemItems_ParentId",
                table: "FileSystemItems",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileSystemItems_FileSystemItems_ParentId",
                table: "FileSystemItems",
                column: "ParentId",
                principalTable: "FileSystemItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileSystemItems_FileSystemItems_ParentId",
                table: "FileSystemItems");

            migrationBuilder.DropIndex(
                name: "IX_FileSystemItems_ParentId",
                table: "FileSystemItems");
        }
    }
}
