using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceSphere.repositery.Data.Migrations
{
    public partial class contractpost2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Terms",
                table: "PostContracts",
                newName: "WorkPlan");

            migrationBuilder.RenameColumn(
                name: "ServiceDetails",
                table: "PostContracts",
                newName: "Timeframe");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "PostContracts",
                newName: "Budget");

            migrationBuilder.AddColumn<string>(
                name: "Availability",
                table: "PostContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "PostContracts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverLetter",
                table: "PostContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FreelancerId",
                table: "PostContracts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Milestones",
                table: "PostContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PostContracts_ClientId",
                table: "PostContracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContracts_FreelancerId",
                table: "PostContracts",
                column: "FreelancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostContracts_AppUser_ClientId",
                table: "PostContracts",
                column: "ClientId",
                principalTable: "AppUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostContracts_AppUser_FreelancerId",
                table: "PostContracts",
                column: "FreelancerId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostContracts_AppUser_ClientId",
                table: "PostContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostContracts_AppUser_FreelancerId",
                table: "PostContracts");

            migrationBuilder.DropIndex(
                name: "IX_PostContracts_ClientId",
                table: "PostContracts");

            migrationBuilder.DropIndex(
                name: "IX_PostContracts_FreelancerId",
                table: "PostContracts");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "PostContracts");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "PostContracts");

            migrationBuilder.DropColumn(
                name: "CoverLetter",
                table: "PostContracts");

            migrationBuilder.DropColumn(
                name: "FreelancerId",
                table: "PostContracts");

            migrationBuilder.DropColumn(
                name: "Milestones",
                table: "PostContracts");

            migrationBuilder.RenameColumn(
                name: "WorkPlan",
                table: "PostContracts",
                newName: "Terms");

            migrationBuilder.RenameColumn(
                name: "Timeframe",
                table: "PostContracts",
                newName: "ServiceDetails");

            migrationBuilder.RenameColumn(
                name: "Budget",
                table: "PostContracts",
                newName: "Price");
        }
    }
}
