using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppMTO.Data.Migrations
{
    public partial class AddedTeachersModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationTeacher_MiddleName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationTeacher_Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationTeacher_Surname",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationTeacher_MiddleName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationTeacher_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationTeacher_Surname",
                table: "AspNetUsers");
        }
    }
}
