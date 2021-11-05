using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetManagementTool_API.Migrations
{
    public partial class RenameEastingAndNorthing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Northing",
                table: "Assets",
                newName: "Longitud");

            migrationBuilder.RenameColumn(
                name: "Easting",
                table: "Assets",
                newName: "Latitude");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitud",
                table: "Assets",
                newName: "Northing");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Assets",
                newName: "Easting");
        }
    }
}
