using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetManagementTool_API.Migrations
{
    public partial class RemoveImageNameFromImageAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "AssetsImages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "AssetsImages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
