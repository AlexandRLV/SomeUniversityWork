using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MTOWebApp.Data.Migrations
{
    public partial class UpdatedAnswerModel1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorrectAnswer",
                table: "QuestionAnswer",
                newName: "Answers2");

            migrationBuilder.AddColumn<string>(
                name: "Answers1",
                table: "QuestionAnswer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answers1",
                table: "QuestionAnswer");

            migrationBuilder.RenameColumn(
                name: "Answers2",
                table: "QuestionAnswer",
                newName: "CorrectAnswer");
        }
    }
}
