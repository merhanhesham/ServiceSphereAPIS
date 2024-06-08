using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceSphere.repositery.Data.Migrations
{
    public partial class FreelancerAddedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AppUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkExperience",
                table: "AppUser",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "WorkExperience",
                table: "AppUser");
        }
    }
}
