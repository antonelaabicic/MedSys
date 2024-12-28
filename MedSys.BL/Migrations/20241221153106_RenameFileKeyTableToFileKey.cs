using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedSys.BL.Migrations
{
    /// <inheritdoc />
    public partial class RenameFileKeyTableToFileKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileKey",
                table: "medicaldocument",
                newName: "file_key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "file_key",
                table: "medicaldocument",
                newName: "FileKey");
        }
    }
}
