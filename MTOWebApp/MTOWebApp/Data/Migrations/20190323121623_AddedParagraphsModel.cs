using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MTOWebApp.Data.Migrations
{
    public partial class AddedParagraphsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Paragraph",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnimFileName = table.Column<string>(nullable: true),
                    FolderName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SoundFileName = table.Column<string>(nullable: true),
                    TextFileName = table.Column<string>(nullable: true),
                    TheoryModuleId = table.Column<int>(nullable: true),
                    VideoFileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paragraph", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paragraph_TheoryModule_TheoryModuleId",
                        column: x => x.TheoryModuleId,
                        principalTable: "TheoryModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paragraph_TheoryModuleId",
                table: "Paragraph",
                column: "TheoryModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paragraph");
        }
    }
}
