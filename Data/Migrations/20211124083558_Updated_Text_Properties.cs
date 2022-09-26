using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseCreatorApp.Data.Migrations
{
    public partial class Updated_Text_Properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_SvgData_SvgDataId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Texts_SvgData_SvgDataId",
                table: "Texts");

            migrationBuilder.AlterColumn<int>(
                name: "SvgDataId",
                table: "Texts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Texts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FontStyle",
                table: "Texts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SvgDataId",
                table: "Images",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_SvgData_SvgDataId",
                table: "Images",
                column: "SvgDataId",
                principalTable: "SvgData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Texts_SvgData_SvgDataId",
                table: "Texts",
                column: "SvgDataId",
                principalTable: "SvgData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_SvgData_SvgDataId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Texts_SvgData_SvgDataId",
                table: "Texts");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Texts");

            migrationBuilder.DropColumn(
                name: "FontStyle",
                table: "Texts");

            migrationBuilder.AlterColumn<int>(
                name: "SvgDataId",
                table: "Texts",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "SvgDataId",
                table: "Images",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_SvgData_SvgDataId",
                table: "Images",
                column: "SvgDataId",
                principalTable: "SvgData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Texts_SvgData_SvgDataId",
                table: "Texts",
                column: "SvgDataId",
                principalTable: "SvgData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
