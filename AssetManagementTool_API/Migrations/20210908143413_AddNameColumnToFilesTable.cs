using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetManagementTool_API.Migrations
{
    public partial class AddNameColumnToFilesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AssetsFiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AssetsFiles");
        }
    }
}
