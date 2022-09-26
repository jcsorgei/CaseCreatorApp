using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseCreatorApp.Data.Migrations
{
    public partial class changedNavigationPropertyToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SvgData_AspNetUsers_AppUserId1",
                table: "SvgData");

            migrationBuilder.DropIndex(
                name: "IX_SvgData_AppUserId1",
                table: "SvgData");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "SvgData");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "SvgData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_SvgData_AppUserId",
                table: "SvgData",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SvgData_AspNetUsers_AppUserId",
                table: "SvgData",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SvgData_AspNetUsers_AppUserId",
                table: "SvgData");

            migrationBuilder.DropIndex(
                name: "IX_SvgData_AppUserId",
                table: "SvgData");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "SvgData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "SvgData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SvgData_AppUserId1",
                table: "SvgData",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SvgData_AspNetUsers_AppUserId1",
                table: "SvgData",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
