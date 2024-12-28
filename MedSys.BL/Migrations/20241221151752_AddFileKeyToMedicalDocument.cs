using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedSys.BL.Migrations
{
    /// <inheritdoc />
    public partial class AddFileKeyToMedicalDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                        name: "FileKey",
                        table: "medicaldocument",
                        type: "text",
                        nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "FileKey",
            table: "medicaldocument");
        }
    }
}
