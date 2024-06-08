using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceSphere.repositery.Data.Migrations
{
    public partial class Posting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPostings_AppUser_ClientId",
                table: "ProjectPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePostings_AppUser_ClientId",
                table: "ServicePostings");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "ServicePostings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "ProjectPostings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPostings_AppUser_ClientId",
                table: "ProjectPostings",
                column: "ClientId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePostings_AppUser_ClientId",
                table: "ServicePostings",
                column: "ClientId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPostings_AppUser_ClientId",
                table: "ProjectPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePostings_AppUser_ClientId",
                table: "ServicePostings");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "ServicePostings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "ProjectPostings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPostings_AppUser_ClientId",
                table: "ProjectPostings",
                column: "ClientId",
                principalTable: "AppUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePostings_AppUser_ClientId",
                table: "ServicePostings",
                column: "ClientId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }
    }
}
