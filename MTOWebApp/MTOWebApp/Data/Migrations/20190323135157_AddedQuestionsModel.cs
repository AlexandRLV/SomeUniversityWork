using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MTOWebApp.Data.Migrations
{
    public partial class AddedQuestionsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CorrectAnswer = table.Column<string>(nullable: true),
                    Task = table.Column<string>(nullable: true),
                    TestModuleId = table.Column<int>(nullable: true),
                    TheoryModuleId = table.Column<int>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_TestModule_TestModuleId",
                        column: x => x.TestModuleId,
                        principalTable: "TestModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Question_TheoryModule_TheoryModuleId",
                        column: x => x.TheoryModuleId,
                        principalTable: "TheoryModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_TestModuleId",
                table: "Question",
                column: "TestModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_TheoryModuleId",
                table: "Question",
                column: "TheoryModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Question");
        }
    }
}
