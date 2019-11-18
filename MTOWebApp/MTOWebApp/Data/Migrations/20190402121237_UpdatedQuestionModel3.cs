using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MTOWebApp.Data.Migrations
{
    public partial class UpdatedQuestionModel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorrectAnswer",
                table: "Question",
                newName: "Answers2");

            migrationBuilder.AddColumn<string>(
                name: "Answers1",
                table: "Question",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answers1",
                table: "Question");

            migrationBuilder.RenameColumn(
                name: "Answers2",
                table: "Question",
                newName: "CorrectAnswer");
        }
    }
}
