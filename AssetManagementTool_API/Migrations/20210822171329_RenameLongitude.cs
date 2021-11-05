using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetManagementTool_API.Migrations
{
    public partial class RenameLongitude : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitud",
                table: "Assets",
                newName: "Longitude");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Assets",
                newName: "Longitud");
        }
    }
}
