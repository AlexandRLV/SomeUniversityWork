using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppMTO.Data.Migrations
{
    public partial class AddedTestScoresModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestScore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationStudentId = table.Column<string>(nullable: true),
                    TestModuleId = table.Column<int>(nullable: true),
                    TestDate = table.Column<DateTime>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestScore_AspNetUsers_ApplicationStudentId",
                        column: x => x.ApplicationStudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestScore_TestModule_TestModuleId",
                        column: x => x.TestModuleId,
                        principalTable: "TestModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestScore_ApplicationStudentId",
                table: "TestScore",
                column: "ApplicationStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TestScore_TestModuleId",
                table: "TestScore",
                column: "TestModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestScore");
        }
    }
}
