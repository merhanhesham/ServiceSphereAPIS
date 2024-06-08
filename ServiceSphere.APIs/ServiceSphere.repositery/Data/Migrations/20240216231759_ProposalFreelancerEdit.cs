using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceSphere.repositery.Data.Migrations
{
    public partial class ProposalFreelancerEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_AppUser_FreelancerId",
                table: "Proposals");

            migrationBuilder.DropIndex(
                name: "IX_Proposals_FreelancerId",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "FreelancerId",
                table: "Proposals");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "Proposals");

            migrationBuilder.AddColumn<string>(
                name: "FreelancerId",
                table: "Proposals",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_FreelancerId",
                table: "Proposals",
                column: "FreelancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_AppUser_FreelancerId",
                table: "Proposals",
                column: "FreelancerId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }
    }
}
