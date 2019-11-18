using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MTOWebApp.Data.Migrations
{
    public partial class UpdatedParModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FolderName",
                table: "Paragraph");

            migrationBuilder.RenameColumn(
                name: "TextFileName",
                table: "Paragraph",
                newName: "PictureFileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PictureFileName",
                table: "Paragraph",
                newName: "TextFileName");

            migrationBuilder.AddColumn<string>(
                name: "FolderName",
                table: "Paragraph",
                nullable: true);
        }
    }
}
