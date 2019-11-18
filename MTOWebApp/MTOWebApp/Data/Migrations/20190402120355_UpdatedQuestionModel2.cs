using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MTOWebApp.Data.Migrations
{
    public partial class UpdatedQuestionModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_TheoryModule_TheoryModuleId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_TheoryModuleId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "TheoryModuleId",
                table: "Question");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TheoryModuleId",
                table: "Question",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_TheoryModuleId",
                table: "Question",
                column: "TheoryModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_TheoryModule_TheoryModuleId",
                table: "Question",
                column: "TheoryModuleId",
                principalTable: "TheoryModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
