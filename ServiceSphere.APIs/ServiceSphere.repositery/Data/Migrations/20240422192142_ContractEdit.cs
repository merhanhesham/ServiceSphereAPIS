using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceSphere.repositery.Data.Migrations
{
    public partial class ContractEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Proposals_ProposalId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ProposalId",
                table: "Contracts");

            migrationBuilder.AlterColumn<int>(
                name: "ProposalId",
                table: "Contracts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "FreelancerId",
                table: "Contracts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_FreelancerId",
                table: "Contracts",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProposalId",
                table: "Contracts",
                column: "ProposalId",
                unique: true,
                filter: "[ProposalId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_AppUser_FreelancerId",
                table: "Contracts",
                column: "FreelancerId",
                principalTable: "AppUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Proposals_ProposalId",
                table: "Contracts",
                column: "ProposalId",
                principalTable: "Proposals",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_AppUser_FreelancerId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Proposals_ProposalId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_FreelancerId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ProposalId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "FreelancerId",
                table: "Contracts");

            migrationBuilder.AlterColumn<int>(
                name: "ProposalId",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProposalId",
                table: "Contracts",
                column: "ProposalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Proposals_ProposalId",
                table: "Contracts",
                column: "ProposalId",
                principalTable: "Proposals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
