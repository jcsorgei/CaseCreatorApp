using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseCreatorApp.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SvgData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Base64 = table.Column<string>(type: "TEXT", nullable: true),
                    Svg = table.Column<string>(type: "TEXT", nullable: true),
                    TemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    TemplateColor = table.Column<string>(type: "TEXT", nullable: true),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SvgData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SvgData_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Base64 = table.Column<string>(type: "TEXT", nullable: true),
                    RotateDeg = table.Column<double>(type: "REAL", nullable: false),
                    ScaleFactor = table.Column<double>(type: "REAL", nullable: false),
                    CurrentTransformationId = table.Column<int>(type: "INTEGER", nullable: true),
                    SvgDataId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Points_CurrentTransformationId",
                        column: x => x.CurrentTransformationId,
                        principalTable: "Points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Images_SvgData_SvgDataId",
                        column: x => x.SvgDataId,
                        principalTable: "SvgData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Texts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TextValue = table.Column<string>(type: "TEXT", nullable: true),
                    RotateDeg = table.Column<double>(type: "REAL", nullable: false),
                    ScaleFactor = table.Column<double>(type: "REAL", nullable: false),
                    CurrentTransformationId = table.Column<int>(type: "INTEGER", nullable: true),
                    SvgDataId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Texts_Points_CurrentTransformationId",
                        column: x => x.CurrentTransformationId,
                        principalTable: "Points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Texts_SvgData_SvgDataId",
                        column: x => x.SvgDataId,
                        principalTable: "SvgData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CurrentTransformationId",
                table: "Images",
                column: "CurrentTransformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_SvgDataId",
                table: "Images",
                column: "SvgDataId");

            migrationBuilder.CreateIndex(
                name: "IX_SvgData_AppUserId",
                table: "SvgData",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Texts_CurrentTransformationId",
                table: "Texts",
                column: "CurrentTransformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Texts_SvgDataId",
                table: "Texts",
                column: "SvgDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Texts");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "SvgData");
        }
    }
}
