using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedSys.BL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFilePathAndFileDataFromMedicalDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_path",
                table: "medicaldocument");

            migrationBuilder.DropColumn(
                name: "filedata",
                table: "medicaldocument");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "file_path",
                table: "medicaldocument",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "filedata",
                table: "medicaldocument",
                type: "bytea",
                nullable: true);
        }
    }
}
