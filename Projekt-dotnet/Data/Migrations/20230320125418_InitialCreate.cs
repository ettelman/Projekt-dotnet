using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_dotnet.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Cd",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Cd",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Cd");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Cd",
                newName: "Image");
        }
    }
}
