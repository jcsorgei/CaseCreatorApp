using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseCreatorApp.Data.Migrations
{
    public partial class datamodel_changed : Migration
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
