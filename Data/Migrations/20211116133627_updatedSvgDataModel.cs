using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseCreatorApp.Data.Migrations
{
    public partial class updatedSvgDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "SvgData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SvgData");
        }
    }
}
